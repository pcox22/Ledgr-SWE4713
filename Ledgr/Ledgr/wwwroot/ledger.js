(() => {
    'use strict';

    /* ---------- Helpers ---------- */
    const $  = (sel, root = document) => root.querySelector(sel);
    const $$ = (sel, root = document) => [...root.querySelectorAll(sel)];
    const esc = (s='') => s.replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
    const num = (n) => Number(n || 0);
    const fmtMoney = (n) => num(n).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    const fmtLongDate = (d) => d.toLocaleDateString(undefined, { year:'numeric', month:'long', day:'numeric' });

    /* ---------- Elements ---------- */
    const els = {
        // Header
        userMenuBtn: $('#userMenuBtn'),
        userMenu: $('#userMenu'),
        logoutBtn: $('#logoutBtn'),

        // Date badge
        dateBtn: $('#dateBtn'),
        dateText: $('#dateText'),
        dateInput: $('#dateInput'),

        // Filters & actions
        dateFrom: $('#dateFrom'),
        dateTo: $('#dateTo'),
        searchBox: $('#searchBox'),
        clearFiltersBtn: $('#clearFiltersBtn'),
        resetFiltersBtn: $('#resetFiltersBtn'),
        exportCsvBtn: $('#exportCsvBtn'),

        // Account card
        acctCard: $('#accountCard'),
        acctName: $('#acctName'),
        acctNumber: $('#acctNumber'),
        acctNormal: $('#acctNormal'),
        acctCategory: $('#acctCategory'),

        // Table
        tbody: $('#ledgerTbody'),
        totalDebit: $('#totalDebit'),
        totalCredit: $('#totalCredit'),
        endingBalance: $('#endingBalance'),

        // Empty state
        empty: $('#emptyState')
    };

    /* ---------- State ---------- */
    const state = {
        account: null,
        entries: [],
        filtered: []
    };

    /* ---------- Demo Fallback Data ---------- */
    const demoAccount = { id:'1001', number:'1001', name:'Cash', normal:'Debit', category:'Current Asset' };
    const demoEntries = [
        { date:'2025-09-01', desc:'Opening balance',            debit:2500, credit:0,   pr:'JE-0001' },
        { date:'2025-09-04', desc:'Client payment INV-1024',    debit:1600, credit:0,   pr:'JE-0007' },
        { date:'2025-09-10', desc:'Office supplies',            debit:0,    credit:120, pr:'JE-0011' },
        { date:'2025-09-14', desc:'Bank fee',                   debit:0,    credit:10,  pr:'JE-0015' },
        { date:'2025-09-20', desc:'Client payment INV-1030',    debit:900,  credit:0,   pr:'JE-0022' },
        { date:'2025-09-28', desc:'Equipment purchase',         debit:0,    credit:500, pr:'JE-0036' }
    ];

    /* ---------- Precompute running balance across full dataset ---------- */
    function precomputeRunningFull() {
        const normal = (state.account?.normal || 'Debit').toLowerCase();
        const sign = normal === 'credit' ? -1 : 1;

        let running = 0;
        state.entries
            .slice()
            .sort((a, b) => a.date.localeCompare(b.date) || (a.pr || '').localeCompare(b.pr || ''))
            .forEach(r => {
                running += sign * (num(r.debit) - num(r.credit));
                r._runningFull = running;
            });
    }

    /* ---------- Init ---------- */
    function init() {
        initHeaderMenu();
        initDateBadge();

        // Account context from URL (fallback)
        const q = new URLSearchParams(location.search);
        state.account = {
            id:       q.get('id')       || demoAccount.id,
            number:   q.get('number')   || demoAccount.number,
            name:     q.get('name')     || demoAccount.name,
            normal:   q.get('normal')   || demoAccount.normal,
            category: q.get('category') || demoAccount.category
        };
        renderAccountCard();

        // Data
        state.entries = Array.isArray(window.__LEDGER_DATA__) ? window.__LEDGER_DATA__ : demoEntries;
        precomputeRunningFull();

        wireFilters();
        applyFilters();
    }

    /* ---------- Header dropdown ---------- */
    function initHeaderMenu(){
        if (!els.userMenuBtn || !els.userMenu) return;

        els.userMenuBtn.addEventListener('click', (e)=>{
            e.stopPropagation();
            els.userMenu.classList.toggle('is-open');
            els.userMenuBtn.setAttribute('aria-expanded', els.userMenu.classList.contains('is-open'));
        });

        document.addEventListener('click', (e)=>{
            if (!els.userMenu.contains(e.target) && !els.userMenuBtn.contains(e.target)) {
                els.userMenu.classList.remove('is-open');
                els.userMenuBtn.setAttribute('aria-expanded', 'false');
            }
        });

        els.logoutBtn?.addEventListener('click', ()=>{
            // UPDATE IN OTHER FILES. LOGOUT BUTTON NOW WORKS!
            window.location.href = 'login.html';
        });
    }

    /* ---------- Date badge & native picker ---------- */
    // UPDATE IN OTHER FILES. CALENDAR NOW WORKS! 
    function initDateBadge(){
        if (!els.dateBtn || !els.dateInput || !els.dateText) return;

        const now = new Date();
        els.dateText.textContent = fmtLongDate(now);
        els.dateInput.valueAsDate = now;

        els.dateBtn.addEventListener('click', ()=>{
            if (typeof els.dateInput.showPicker === 'function') els.dateInput.showPicker();
            else els.dateInput.focus();
        });

        els.dateInput.addEventListener('change', ()=>{
            const d = els.dateInput.valueAsDate || new Date(els.dateInput.value);
            if (!isNaN(d)) els.dateText.textContent = fmtLongDate(d);
            els.dateInput.blur();
        });

        document.addEventListener('pointerdown', (e)=>{
            if (!els.dateBtn.contains(e.target) && e.target !== els.dateInput) {
                els.dateInput.blur();
            }
        });
        document.addEventListener('keydown', (e)=>{
            if (e.key === 'Escape') els.dateInput.blur();
        });
    }

    /* ---------- UI Wiring ---------- */
    function wireFilters() {
        els.dateFrom.addEventListener('change', applyFilters);
        els.dateTo.addEventListener('change', applyFilters);
        els.searchBox.addEventListener('input', applyFilters);

        els.clearFiltersBtn.addEventListener('click', () => {
            els.dateFrom.value = '';
            els.dateTo.value = '';
            els.searchBox.value = '';
            applyFilters();
        });

        els.resetFiltersBtn.addEventListener('click', () => {
            els.dateFrom.value = '';
            els.dateTo.value = '';
            els.searchBox.value = '';
            applyFilters();
        });

        els.exportCsvBtn.addEventListener('click', exportCsv);
    }

    /* ---------- Account Header ---------- */
    function renderAccountCard() {
        const a = state.account;
        if (!a) return;
        els.acctName.textContent = a.name || '—';
        els.acctNumber.textContent = a.number ? `#${a.number}` : '—';
        els.acctNormal.textContent = a.normal || '—';
        els.acctCategory.textContent = a.category || '—';
        els.acctCard.classList.remove('hide');
    }

    /* ---------- Filtering + Render ---------- */
    function applyFilters() {
        const from = els.dateFrom.value ? new Date(els.dateFrom.value) : null;
        const to   = els.dateTo.value   ? new Date(els.dateTo.value)   : null;
        const q    = (els.searchBox.value || '').trim().toLowerCase();

        state.filtered = state.entries.filter(r => {
            const d = new Date(r.date);
            if (from && d < from) return false;
            if (to && d > to) return false;
            if (q) {
                const hit =
                    (state.account?.name || '').toLowerCase().includes(q) ||
                    (r.pr || '').toLowerCase().includes(q) ||
                    String(r.debit || '').includes(q) ||
                    String(r.credit || '').includes(q) ||
                    (r.desc || '').toLowerCase().includes(q);
                if (!hit) return false;
            }
            return true;
        });

        renderTable(state.filtered);
    }

    function renderTable(rows) {
        els.tbody.innerHTML = '';

        // sums for the filtered view only
        let sumDr = 0, sumCr = 0;

        // always show table in chronological order
        const sorted = rows.slice().sort((a,b)=> a.date.localeCompare(b.date) || (a.pr||'').localeCompare(b.pr||''));

        sorted.forEach(r => {
            const dr = num(r.debit), cr = num(r.credit);
            sumDr += dr; sumCr += cr;

            const tr = document.createElement('tr');
            tr.innerHTML = `
      <td>${r.date}</td>
      <td>${esc(r.desc || '')}</td>
      <td class="num">${dr ? fmtMoney(dr) : ''}</td>
      <td class="num">${cr ? fmtMoney(cr) : ''}</td>
      <td class="num">${fmtMoney(r._runningFull || 0)}</td>
      <td><a class="link" href="journal-entry.html?id=${encodeURIComponent(r.pr)}" title="Open journal ${esc(r.pr || '')}">${esc(r.pr || '')}</a></td>
    `;
            if (cr > 0) tr.classList.add('credit');
            els.tbody.appendChild(tr);
        });

        els.totalDebit.textContent  = fmtMoney(sumDr);
        els.totalCredit.textContent = fmtMoney(sumCr);

        // Ending Balance = running balance at the last visible row (true account balance at that point)
        const last = sorted[sorted.length - 1];
        els.endingBalance.textContent = fmtMoney(last ? (last._runningFull || 0) : 0);

        // empty state
        els.empty.classList.toggle('hide', rows.length !== 0);
    }

    /* ---------- Export CSV ---------- */
    function exportCsv() {
        const normal = (state.account?.normal || 'Debit').toLowerCase();
        const sign = normal === 'credit' ? -1 : 1;

        const out = [['Date','Description','Debit','Credit','Running Balance','PR']];
        let running = 0;

        state.filtered.slice().sort((a,b)=>a.date.localeCompare(b.date)).forEach(r=>{
            const dr = num(r.debit), cr = num(r.credit);
            running += sign * (dr - cr);
            out.push([ r.date, (r.desc||'').replace(/\r?\n/g,' '), dr?dr.toFixed(2):'', cr?cr.toFixed(2):'', running.toFixed(2), r.pr||'' ]);
        });

        const csv = out.map(row => row.map(v => (typeof v==='string' && /[",\n]/.test(v)) ? `"${v.replace(/"/g,'""')}"` : v).join(',')).join('\n');
        const blob = new Blob([csv], { type:'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url; a.download = `ledger_${state.account?.number||'account'}.csv`;
        document.body.appendChild(a); a.click(); URL.revokeObjectURL(url); a.remove();
    }

    /* ---------- Start ---------- */
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else init();
})();