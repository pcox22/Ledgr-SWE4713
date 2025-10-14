// ============================
// Accounts module (COA List)
// ============================

// tiny DOM helpers 
window.$  = window.$  || (sel => document.querySelector(sel));
window.$$ = window.$$ || (sel => Array.from(document.querySelectorAll(sel)));
const on = (el, ev, fn) => el && el.addEventListener(ev, fn);

// money format helper (2 decimals + thousands)
window.formatMoney = window.formatMoney || function(n){
    const num = Number(n ?? 0);
    return num.toLocaleString(undefined, {minimumFractionDigits:2, maximumFractionDigits:2});
};

window.coa = window.coa || {};

// ---- current user role (comes from app.js; default Admin) ----
const CURRENT_ROLE = (window.currentUserRole || 'Admin');


/**
 * Initialize Chart of Accounts list page
 * @param {Object} cfg
 */
window.coa.initListPage = function initListPage(cfg){
    const els = {
        applyDates: $('#btnApplyDates'),
        tbody: $(cfg.tableSel),
        alert: $(cfg.alertSel),
        alertText: $(cfg.alertTextSel),
        exportBtn: $(cfg.exportBtn),
        clearBtn: $(cfg.clearBtn),
        form: $(cfg.formSel),
        dateFrom: $(cfg.dateFrom),
        dateTo: $(cfg.dateTo),
        search: $(cfg.searchSel),
        pgPrev: $(cfg.pagination?.prev),
        pgNext: $(cfg.pagination?.next),
        pgStatus: $(cfg.pagination?.status),
        addBtn: $(cfg.addBtn)
    };

    // role-based UI (hide Add button for non-admins)
    if (CURRENT_ROLE !== 'Admin' && els.addBtn) {
        els.addBtn.style.display = 'none';
        document.body.classList.add('role-readonly');
    }

    const role = (window.currentUserRole || 'Admin');
    if(role !== 'Admin' && els.addBtn){ els.addBtn.style.display = 'none'; }

    let state = { page:1, pageSize:10, totalPages:1, rows:[], rowsAll:[], minDate:null, maxDate:null };

    // Filters
    on(els.form, 'submit', e => { e.preventDefault(); state.page = 1; load(); });
    on(els.clearBtn, 'click', () => {
        // reset all form fields
        els.form.reset();
        // reset date inputs to dataset bounds rather than blank
        if (els.dateFrom) els.dateFrom.value = '';
        if (els.dateTo)   els.dateTo.value   = '';
        if (state.minDate && els.dateFrom) els.dateFrom.valueAsDate = state.minDate;
        if (state.maxDate && els.dateTo)   els.dateTo.valueAsDate   = state.maxDate;
        if (els.search) els.search.value = '';
        state.page = 1;
        load();
    });

    // Pagination
    on(els.pgPrev, 'click', () => { if(state.page>1){ state.page--; render(); }});
    on(els.pgNext, 'click', () => { if(state.page<state.totalPages){ state.page++; render(); }});

    // Export
    on(els.exportBtn, 'click', () => exportCsv(state.rowsAll));

    // Apply Dates
    on(els.applyDates, 'click', () => { state.page = 1; load(); });

    // First load
    load();

    // ----------------- data load -----------------
    async function load(){
        try{
            showAlert(false);

            // 1) always fetch the full dataset
            const all = await fetchAccountsMock();

            // 2) compute min/max from ALL data (created date)
            const createdDates = all
                .map(r => r.created)
                .filter(Boolean)
                .map(d => new Date(d + 'T00:00:00'));
            if (createdDates.length){
                const min = new Date(Math.min.apply(null, createdDates));
                const max = new Date(Math.max.apply(null, createdDates));
                state.minDate = min; state.maxDate = max;

                // if inputs are empty, set them to bounds
                if (els.dateFrom && !els.dateFrom.value) els.dateFrom.valueAsDate = min;
                if (els.dateTo   && !els.dateTo.value)   els.dateTo.valueAsDate   = max;
            }

            // 3) read current filter UI
            const q = {
                category: $('#fCategory')?.value || '',
                subcategory: $('#fSubcategory')?.value || '',
                side: $('#fNormalSide')?.value || '',
                acctFrom: Number($('#fAcctFrom')?.value || 0),
                acctTo: Number($('#fAcctTo')?.value || 0),
                amtMin: Number($('#fAmtMin')?.value || 0),
                amtMax: Number($('#fAmtMax')?.value || 0),
                search: els.search?.value?.trim() || '',
                from: els.dateFrom?.value || '',
                to:   els.dateTo?.value   || ''
            };

            // 4) filter
            const filtered = all.filter(r => {
                if(q.category && r.category !== q.category) return false;
                if(q.subcategory && !r.subcategory.toLowerCase().includes(q.subcategory.toLowerCase())) return false;
                if(q.side && r.normalSide !== q.side) return false;
                if(q.acctFrom && r.number < q.acctFrom) return false;
                if(q.acctTo && r.number > q.acctTo) return false;
                if(q.amtMin && r.balance < q.amtMin) return false;
                if(q.amtMax && r.balance > q.amtMax) return false;
                if(q.search){
                    const s=q.search.toLowerCase();
                    if(!(`${r.number}`.includes(s) || r.name.toLowerCase().includes(s))) return false;
                }

                // Date range filter (by created date)
                if (q.from || q.to){
                    const created = r.created ? new Date(r.created + 'T00:00:00') : null;
                    if (!created) return false;
                    const from = q.from ? new Date(q.from + 'T00:00:00') : null;
                    const to   = q.to   ? new Date(q.to   + 'T23:59:59') : null; // inclusive end
                    if (from && created < from) return false;
                    if (to   && created > to)   return false;
                }

                return true;
            });

            state.rowsAll = filtered;
            state.totalPages = Math.max(1, Math.ceil(filtered.length / state.pageSize));
            render();
        }catch(err){
            console.error(err);
            showAlert(true, 'There was a problem loading accounts.');
        }
    }

    // ----------------- render -----------------
    function render(){
        const start = (state.page-1)*state.pageSize;
        const rows = state.rowsAll.slice(start, start+state.pageSize);
        state.rows = rows;

        els.tbody.innerHTML = rows.length ? rows.map(rowHtml).join('') :
            `<tr class="muted"><td colspan="17" style="text-align:center; padding:24px;">No results</td></tr>`;

        // Click-through to ledger
        $$('#coaRows tr[data-acct]').forEach(tr => {
            on(tr, 'click', () => {
                const acct = tr.getAttribute('data-acct');
                window.location.href = `../ledger/ledger-account.html?acct=${acct}`;
            });
        });

        // Pagination controls
        els.pgPrev.disabled = (state.page<=1);
        els.pgNext.disabled = (state.page>=state.totalPages);
        els.pgStatus.textContent = `Page ${state.page} of ${state.totalPages}`;
    }

    function rowHtml(r){
        const money = v => `$${formatMoney(v)}`;
        const editCell = (CURRENT_ROLE === 'Admin')
            ? `<td class="text-right">
         <button class="btn btn--secondary btn--small"
                 onclick="event.stopPropagation(); window.location.href='./coa-form.html?id=${r.id}'">
           Edit
         </button>
       </td>`
            : ``;

        return `
    <tr class="table__row" data-acct="${r.number}" style="cursor:pointer">
      <td>${r.number}</td>
      <td><a class="link" href="../ledger/ledger-account.html?acct=${r.number}" onclick="event.stopPropagation()">${r.name}</a></td>
      <td>${r.description || ''}</td>
      <td>${r.normalSide}</td>
      <td>${r.category}</td>
      <td>${r.subcategory}</td>
      <td>${money(r.initialBalance)}</td>
      <td>${money(r.debit)}</td>
      <td>${money(r.credit)}</td>
      <td>${money(r.balance)}</td>
      <td>${r.statement}</td>
      <td>${r.order || ''}</td>
      <td>${r.addedBy || ''}</td>
      <td>${r.created || ''}</td>
      <td>${r.updated || ''}</td>
      <td>${r.status || 'Active'}</td>
      ${editCell}
    </tr>`;
    }


    function exportCsv(rows){
        if(!rows || !rows.length){ return showAlert(true, 'Nothing to export.'); }
        const headers = [
            'Account No','Name','Description','Normal Side','Category','Subcategory',
            'Initial Balance','Debit','Credit','Balance','Statement','Order','Added By','Created','Updated','Status'
        ];
        const lines = rows.map(r => [
            r.number, r.name, r.description, r.normalSide, r.category, r.subcategory,
            r.initialBalance, r.debit, r.credit, r.balance, r.statement, r.order, r.addedBy, r.created, r.updated, r.status
        ].map(v => `"${(v??'').toString().replace(/"/g,'""')}"`).join(','));
        const csv = [headers.join(','), ...lines].join('\n');
        const blob = new Blob([csv], {type:'text/csv;charset=utf-8;'});
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url; a.download = 'chart-of-accounts.csv';
        document.body.appendChild(a); a.click(); a.remove();
        URL.revokeObjectURL(url);
    }

    function showAlert(show, text){
        if(!els.alert) return;
        els.alert.classList.toggle('is-hidden', !show);
        if(text && els.alertText) els.alertText.textContent = text;
    }

    // ----------------- MOCK DATA (swap with real API) -----------------
    async function fetchAccountsMock(){
        await new Promise(r => setTimeout(r, 120)); // tiny delay
        return [
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
};