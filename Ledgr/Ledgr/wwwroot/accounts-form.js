// ============================
// Accounts: Add/Edit Form
// ============================

window.accountsForm = (function(){
    // ---- Simple DOM helpers ----
    const $  = window.$  || (sel => document.querySelector(sel));
    const $$ = window.$$ || (sel => Array.from(document.querySelectorAll(sel)));

    // increase header shadow after the user scrolls
    (function(){
        const onScroll = () => {
            if (window.scrollY > 8) {
                document.body.classList.add('scrolled');
            } else {
                document.body.classList.remove('scrolled');
            }
        };

        window.addEventListener('scroll', onScroll, { passive: true });
        onScroll();
    })();


    // ---- Mock dataset ----
    if(!window.__mockAccounts){
        window.__mockAccounts = [
            { id:1, number:101, name:'Cash', description:'Cash on hand', normalSide:'Debit', category:'Asset', subcategory:'Current Assets',
                initialBalance:5000, debit:2500, credit:1000, balance:6500, statement:'BS', order:1, addedBy:'admin', created:'2025-10-01', updated:'2025-10-10', status:'Active' },
            { id:2, number:201, name:'Accounts Payable', description:'Vendors', normalSide:'Credit', category:'Liability', subcategory:'Current Liabilities',
                initialBalance:200, debit:0, credit:50, balance:250, statement:'BS', order:5, addedBy:'manager1', created:'2025-10-02', updated:'2025-10-09', status:'Active' },
            { id:3, number:410, name:'Service Revenue', description:'Client services', normalSide:'Credit', category:'Revenue', subcategory:'Operating Revenue',
                initialBalance:0, debit:0, credit:12860, balance:12860, statement:'IS', order:10, addedBy:'admin', created:'2025-10-04', updated:'2025-10-11', status:'Active' },
            { id:4, number:510, name:'Rent Expense', description:'Office rent', normalSide:'Debit', category:'Expense', subcategory:'Operating Expense',
                initialBalance:0, debit:3200, credit:0, balance:3200, statement:'IS', order:11, addedBy:'admin', created:'2025-10-04', updated:'2025-10-11', status:'Active' }
        ];
    }

    // ---- Event log in localStorage ----
    const LOG_KEY = 'coaEventLog';
    const readLog = () => JSON.parse(localStorage.getItem(LOG_KEY) || '[]');
    const writeLog = (arr) => localStorage.setItem(LOG_KEY, JSON.stringify(arr));

    // ---- Category → starting digit rule ----
    const START_DIGIT = { Asset:'1', Liability:'2', Equity:'3', Revenue:'4', Expense:'5' };

    function init(){
        // Elements
        const f = {
            form: $('#accountForm'),
            alert: $('#formAlert'),
            alertText: $('#formAlertText'),
            id: null,
            accountNo: $('#accountNo'),
            accountName: $('#accountName'),
            description: $('#description'),
            category: $('#category'),
            subcategory: $('#subcategory'),
            normalSide: $('#normalSide'),
            initialBalance: $('#initialBalance'),
            statement: $('#statement'),
            order: $('#order'),
            active: $('#active'),
            comment: $('#comment'),
            btnCancel: $('#btnCancel'),
            btnSave: $('#btnSave'),
            accountNoError: $('#accountNoError'),
            accountNameError: $('#accountNameError'),
            activeHint: $('#activeHint'),
            eventRows: $('#eventRows')
        };

        // parse id from URL (?id=)
        const params = new URLSearchParams(location.search);
        const id = Number(params.get('id') || 0);
        f.id = id > 0 ? id : null;

        // bind events
        f.category.addEventListener('change', () => hintStartingDigit(f));
        f.accountNo.addEventListener('input', () => enforceNumeric(f));
        f.accountNo.addEventListener('blur', () => validateAccountNo(f));
        f.accountName.addEventListener('blur', () => validateAccountName(f));

        f.active.addEventListener('change', () => enforceActiveRule(f));
        f.btnCancel.addEventListener('click', () => history.back());

        f.form.addEventListener('submit', (e) => {
            e.preventDefault();
            save(f);
        });

        // load data if editing
        if(f.id){
            const rec = window.__mockAccounts.find(a => a.id === f.id);
            if(rec){ fillForm(f, rec); loadEvents(f, rec); }
        }else{
            loadEvents(f, null);
        }

        // initial hint
        hintStartingDigit(f);
    }

    // ----------------- UI helpers -----------------
    function fillForm(f, r){
        f.accountNo.value   = r.number;
        f.accountName.value = r.name;
        f.description.value = r.description || '';
        f.category.value    = r.category || '';
        f.subcategory.value = r.subcategory || '';
        f.normalSide.value  = r.normalSide || '';
        f.initialBalance.value = (r.initialBalance ?? 0).toFixed(2);
        f.statement.value   = r.statement || '';
        f.order.value       = r.order ?? 0;
        f.active.checked    = (r.status ?? 'Active') === 'Active';

        // enforce active rule if balance > 0
        if((r.balance ?? 0) > 0){
            f.active.disabled = true;
            f.activeHint.textContent = 'You cannot deactivate when balance > 0. Current balance: $' + numberFmt(r.balance);
        }
    }

    function loadEvents(f, r){
        const all = readLog();
        const rows = r ? all.filter(x => x.recordId === r.id) : [];
        if(!rows.length){
            f.eventRows.innerHTML = `<tr class="muted"><td colspan="7" style="text-align:center; padding:16px;">No events yet</td></tr>`;
            return;
        }
        f.eventRows.innerHTML = rows.slice(-10).reverse().map(ev => `
      <tr>
        <td>${ev.eventId}</td>
        <td>${ev.action}</td>
        <td>${ev.user || 'admin'}</td>
        <td>${new Date(ev.timestamp).toLocaleString()}</td>
        <td><pre class="mini-json">${safeJson(ev.before)}</pre></td>
        <td><pre class="mini-json">${safeJson(ev.after)}</pre></td>
        <td>${ev.comment || ''}</td>
      </tr>
    `).join('');
    }

    // ----------------- Validation -----------------
    function enforceNumeric(f){
        const original = f.accountNo.value || "";
        const cleaned  = original.replace(/\D+/g, ""); // keep digits only
        if (original !== cleaned) {
            f.accountNo.value = cleaned;
            // visible alert for the admin
            showAlert(f, true, "Account number may contain digits only (0–9). Decimals, letters, spaces and symbols were removed.");
            // auto-hide after a moment so it doesn’t linger
            setTimeout(() => showAlert(f, false), 2500);
        }
    }


    function hintStartingDigit(f){
        const cat = f.category.value;
        const sd = START_DIGIT[cat];
        const hint = $('#acctHint');
        if(sd){
            hint.innerHTML = `Selected: <b>${cat}</b> — account number must start with <b>${sd}</b>.`;
        }else{
            hint.textContent = 'Assets start with 1, Liabilities 2, Equity 3, Revenue 4, Expense 5.';
        }
        // revalidate number if present
        if(f.accountNo.value) validateAccountNo(f);
    }

    function validateAccountNo(f){
        f.accountNoError.textContent = '';
        const val = f.accountNo.value.trim();
        if(!val){ f.accountNoError.textContent = 'Account number is required.'; return false; }
        if(!/^\d+$/.test(val)){ f.accountNoError.textContent = 'Only digits allowed.'; return false; }

        const sd = START_DIGIT[f.category.value];
        if(sd && !val.startsWith(sd)){
            f.accountNoError.textContent = `Must start with ${sd} for category ${f.category.value}.`;
            return false;
        }

        // duplicate check
        const dupe = window.__mockAccounts.find(a => String(a.number) === val && a.id !== f.id);
        if(dupe){
            f.accountNoError.textContent = `Account number already exists (${dupe.number} — ${dupe.name}).`;
            return false;
        }
        return true;
    }

    function validateAccountName(f){
        f.accountNameError.textContent = '';
        const name = f.accountName.value.trim();
        if(!name){ f.accountNameError.textContent = 'Account name is required.'; return false; }
        const dupe = window.__mockAccounts.find(a => a.name.toLowerCase() === name.toLowerCase() && a.id !== f.id);
        if(dupe){
            f.accountNameError.textContent = `Account name already exists (${dupe.name}).`;
            return false;
        }
        return true;
    }

    function enforceActiveRule(f){
        // if user tries to deactivate while balance > 0, revert and show hint
        if(!f.id) return; // new accounts can be toggled freely (balance=initial)
        const rec = window.__mockAccounts.find(a => a.id === f.id);
        if(rec && rec.balance > 0 && !f.active.checked){
            f.active.checked = true;
            showAlert(f, true, 'Cannot deactivate: account has a positive balance.');
        }else{
            showAlert(f, false);
        }
    }

    // ----------------- Save -----------------
    function save(f){
        // basic field-level checks
        const okNo   = validateAccountNo(f);
        const okName = validateAccountName(f);
        if(!okNo || !okName){
            showAlert(f, true, 'Please fix the errors before saving.');
            return;
        }
        if(!f.category.value || !f.normalSide.value || !f.statement.value || !f.initialBalance.value){
            showAlert(f, true, 'Please complete all required fields (*).');
            return;
        }

        const payload = {
            number: Number(f.accountNo.value),
            name: f.accountName.value.trim(),
            description: f.description.value.trim(),
            category: f.category.value,
            subcategory: f.subcategory.value.trim(),
            normalSide: f.normalSide.value,
            initialBalance: Number(f.initialBalance.value || 0),
            statement: f.statement.value,
            order: Number(f.order.value || 0),
            status: f.active.checked ? 'Active' : 'Inactive',
            updated: new Date().toISOString().slice(0,10)
        };

        // Before/After snapshots for event log
        let before = null, after = null, action = 'Add', recordId;
        if(f.id){
            const idx = window.__mockAccounts.findIndex(a => a.id === f.id);
            if(idx >= 0){
                action = 'Edit';
                recordId = f.id;
                before = {...window.__mockAccounts[idx]};
                // merge
                window.__mockAccounts[idx] = { ...window.__mockAccounts[idx], ...payload };
                after  = {...window.__mockAccounts[idx]};
            }
        }else{
            recordId = (Math.max(...window.__mockAccounts.map(a=>a.id)) || 0) + 1;
            const created = new Date().toISOString().slice(0,10);
            const row = { id:recordId, debit:0, credit:0, balance:payload.initialBalance, addedBy:'admin', created, ...payload };
            window.__mockAccounts.push(row);
            before = null; after = {...row};
        }

        // write event log
        const log = readLog();
        log.push({
            eventId: (log.length ? log[log.length-1].eventId+1 : 1001),
            recordId,
            action,
            before, after,
            user: 'admin',
            timestamp: Date.now(),
            comment: ($('#comment').value || '')
        });
        writeLog(log);

        // go back to list
        location.href = './coa-list.html';
    }

    // ----------------- Utilities -----------------
    function showAlert(f, show, text=''){
        if(!f.alert) return;
        f.alert.classList.toggle('is-hidden', !show);
        if(text) f.alertText.textContent = text;
    }
    const numberFmt = (n) => Number(n||0).toLocaleString(undefined,{minimumFractionDigits:2, maximumFractionDigits:2});
    const safeJson = (o) => o ? JSON.stringify(o, null, 2) : '—';

    // expose
    return { init };
})();

function fillForm(f, r){
    f.accountNo.value   = r.number;
    f.accountName.value = r.name;
    f.description.value = r.description || '';
    f.category.value    = r.category || '';
    f.subcategory.value = r.subcategory || '';
    f.normalSide.value  = r.normalSide || '';
    f.initialBalance.value = (r.initialBalance ?? 0).toFixed(2);
    f.debit.value       = (r.debit ?? 0).toFixed(2);
    f.credit.value      = (r.credit ?? 0).toFixed(2);
    f.balance.value     = (r.balance ?? 0).toFixed(2);
    f.statement.value   = r.statement || '';
    f.order.value       = r.order ?? 0;
    f.created.value     = (r.created ? r.created + 'T00:00' : '');
    f.addedBy.value     = r.addedBy || 'admin';
    f.active.checked    = (r.status ?? 'Active') === 'Active';

    // enforce active rule if balance > 0
    if((r.balance ?? 0) > 0){
        f.active.disabled = true;
        f.activeHint.textContent = 'You cannot deactivate when balance > 0. Current balance: $' + numberFmt(r.balance);
    }
}