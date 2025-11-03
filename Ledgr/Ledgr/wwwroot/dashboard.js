const $ = (sel, root=document) => root.querySelector(sel);

/* === User dropdown === */
(() => {
    const btn = $('#userMenuBtn');
    const menu = $('#userMenu');
    if (!btn || !menu) return;

    const toggle = (open) => {
        menu.classList.toggle('is-open', open);
        btn.setAttribute('aria-expanded', String(open));
    };

    btn.addEventListener('click', (e) => {
        e.stopPropagation();
        toggle(!menu.classList.contains('is-open'));
    });

    document.addEventListener('click', (e) => {
        if (!menu.contains(e.target) && e.target !== btn) toggle(false);
    });
    
})();

/* === Calendar === */
(() => {
    const dateBtn   = $('#dateBtn');
    const dateText  = $('#dateText');
    const dateInput = $('#dateInput');
    const popover   = $('#calendarPopover');
    const grid      = $('#calGrid');
    const labelEl   = $('#calLabel');
    if (!dateBtn || !popover) return;

    let view = new Date();
    let selected = new Date();

    const ordinal = (n) => {
        const s = ['th','st','nd','rd'], v = n % 100;
        return n + (s[(v-20)%10] || s[v] || s[0]);
    };
    const setPretty = (d) => {
        const month = d.toLocaleString(undefined, { month: 'long' });
        dateText.textContent = `${month} ${ordinal(d.getDate())}, ${d.getFullYear()}`;
    };
    setPretty(selected);

    const startOfMonth = (d) => new Date(d.getFullYear(), d.getMonth(), 1);
    const endOfMonth   = (d) => new Date(d.getFullYear(), d.getMonth()+1, 0);

    const render = () => {
        labelEl.textContent = view.toLocaleString(undefined, { month: 'long', year: 'numeric' });
        grid.innerHTML = '';

        const start = startOfMonth(view);
        const end   = endOfMonth(view);
        const offset = start.getDay();
        const total = Math.ceil((offset + end.getDate()) / 7) * 7;
        const first = new Date(start);
        first.setDate(start.getDate() - offset);

        for (let i = 0; i < total; i++) {
            const d = new Date(first);
            d.setDate(first.getDate() + i);

            const b = document.createElement('button');
            b.className = 'cal__day';
            b.textContent = d.getDate();

            if (d.getMonth() !== view.getMonth()) b.classList.add('is-muted');

            const today = new Date();
            const todayStr = new Date(today.getFullYear(), today.getMonth(), today.getDate()).toDateString();
            const dStr     = new Date(d.getFullYear(), d.getMonth(), d.getDate()).toDateString();
            const selStr   = new Date(selected.getFullYear(), selected.getMonth(), selected.getDate()).toDateString();

            if (dStr === todayStr) b.classList.add('is-today');
            if (dStr === selStr)   b.classList.add('is-selected');

            b.addEventListener('click', () => {
                selected = new Date(d);
                setPretty(selected);
                hide();
            });

            grid.appendChild(b);
        }
    };

    const position = () => {
        const r = dateBtn.getBoundingClientRect();
        popover.style.left = `${r.left + window.scrollX}px`;
        popover.style.top  = `${r.bottom + 8 + window.scrollY}px`;
    };

    const onDocClick = (e) => {
        if (popover.contains(e.target) || dateBtn.contains(e.target)) return;
        hide();
    };

    const show = () => {
        popover.hidden = false;
        dateBtn.setAttribute('aria-expanded', 'true');
        render(); position();
        document.addEventListener('click', onDocClick);
    };
    const hide = () => {
        popover.hidden = true;
        dateBtn.setAttribute('aria-expanded', 'false');
        document.removeEventListener('click', onDocClick);
    };

    dateBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        popover.hidden ? show() : hide();
    });
    window.addEventListener('resize', () => { if (!popover.hidden) position(); });

    // Month navigation
    document.querySelectorAll('.cal__nav').forEach((btn) => {
        btn.addEventListener('click', () => {
            const dir = Number(btn.dataset.dir || 0);
            view = new Date(view.getFullYear(), view.getMonth() + dir, 1);
            render();
        });
    });

    // Native input sync
    dateInput.addEventListener('change', () => {
        const d = dateInput.value ? new Date(dateInput.value) : new Date();
        selected = d; view = new Date(d.getFullYear(), d.getMonth(), 1);
        setPretty(selected); render();
    });
})();