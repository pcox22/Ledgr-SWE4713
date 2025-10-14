// COA Event Log (admin)
(function(){
    const $ = (s)=>document.querySelector(s);
    const $$= (s)=>Array.from(document.querySelectorAll(s));
    const LOG_KEY = 'coaEventLog';

// ....................................................................................
// MOCK DATA
// ....................................................................................
    if(!localStorage.getItem(LOG_KEY)){
        const now = Date.now();
        const mock = [
            {
                eventId:1001,
                recordId:1,
                action:'Add',
                user:'admin',
                timestamp: now - 7*24*60*60*1000,
                comment:'Initial account setup',
                before:null,
                after:{
                    id:1, number:101, name:'Cash', category:'Asset', subcategory:'Current Assets',
                    normalSide:'Debit', initialBalance:5000, balance:5000, statement:'BS', status:'Active'
                }
            },
            {
                eventId:1002,
                recordId:2,
                action:'Add',
                user:'manager1',
                timestamp: now - 6*24*60*60*1000,
                comment:'Vendor payables account added',
                before:null,
                after:{
                    id:2, number:201, name:'Accounts Payable', category:'Liability',
                    subcategory:'Current Liabilities', normalSide:'Credit',
                    initialBalance:0, balance:0, statement:'BS', status:'Active'
                }
            },
            {
                eventId:1003,
                recordId:3,
                action:'Add',
                user:'admin',
                timestamp: now - 5*24*60*60*1000,
                comment:'Revenue account added for client services',
                before:null,
                after:{
                    id:3, number:410, name:'Service Revenue', category:'Revenue',
                    subcategory:'Operating Revenue', normalSide:'Credit',
                    initialBalance:0, balance:0, statement:'IS', status:'Active'
                }
            },
            {
                eventId:1004,
                recordId:1,
                action:'Edit',
                user:'admin',
                timestamp: now - 4*24*60*60*1000,
                comment:'Increased starting balance after deposit',
                before:{
                    id:1, number:101, name:'Cash', balance:5000
                },
                after:{
                    id:1, number:101, name:'Cash', balance:6500
                }
            },
            {
                eventId:1005,
                recordId:2,
                action:'Edit',
                user:'manager1',
                timestamp: now - 3*24*60*60*1000,
                comment:'Added small payable for supplier invoice',
                before:{
                    id:2, number:201, balance:0
                },
                after:{
                    id:2, number:201, balance:250
                }
            },
            {
                eventId:1006,
                recordId:4,
                action:'Add',
                user:'admin',
                timestamp: now - 2*24*60*60*1000,
                comment:'Expense account created for office rent',
                before:null,
                after:{
                    id:4, number:510, name:'Rent Expense', category:'Expense',
                    subcategory:'Operating Expense', normalSide:'Debit',
                    initialBalance:0, balance:0, statement:'IS', status:'Active'
                }
            },
            {
                eventId:1007,
                recordId:4,
                action:'Edit',
                user:'accountant2',
                timestamp: now - 1*24*60*60*1000,
                comment:'Added first month rent expense',
                before:{
                    id:4, number:510, balance:0
                },
                after:{
                    id:4, number:510, balance:3200
                }
            },
            {
                eventId:1008,
                recordId:3,
                action:'Deactivate',
                user:'admin',
                timestamp: now - 2*60*60*1000,
                comment:'Account no longer used in reporting period',
                before:{
                    id:3, number:410, name:'Service Revenue', status:'Active'
                },
                after:{
                    id:3, number:410, name:'Service Revenue', status:'Inactive'
                }
            }
        ];
        localStorage.setItem(LOG_KEY, JSON.stringify(mock));
    }
// ....................................................................................

    const els = {
        rows:   $('#logRows'),
        from:   $('#lfFrom'),
        to:     $('#lfTo'),
        action: $('#lfAction'),
        user:   $('#lfUser'),
        search: $('#lfSearch'),
        clear:  $('#lfClear'),
        form:   $('#logFilters'),
        prev:   $('#logPrev'),
        next:   $('#logNext'),
        status: $('#logStatus'),
        export: $('#logExport')
    };

    let state = { page:1, pageSize:12, totalPages:1, rowsAll:[], rowsPage:[] };

    // data
    const readLog = () => JSON.parse(localStorage.getItem(LOG_KEY) || '[]');
    const accounts = (window.__mockAccounts || []);

    // events
    els.form.addEventListener('submit', e => { e.preventDefault(); state.page=1; load(); });
    els.clear.addEventListener('click', () => { els.form.reset(); setDateBounds(); state.page=1; load(); });
    els.prev.addEventListener('click', () => { if(state.page>1){ state.page--; render(); } });
    els.next.addEventListener('click', () => { if(state.page<state.totalPages){ state.page++; render(); } });
    els.export.addEventListener('click', exportCsv);

    // init
    setDateBounds();
    load();

    function setDateBounds(){
        const log = readLog();
        if(!log.length) return;
        const times = log.map(x=>x.timestamp).filter(Boolean);
        const min = new Date(Math.min.apply(null, times));
        const max = new Date(Math.max.apply(null, times));
        if(els.from && !els.from.value) els.from.valueAsDate = min;
        if(els.to   && !els.to.value)   els.to.valueAsDate   = max;
    }

    function load(){
        const log = readLog();
        const q = {
            from:  els.from?.value || '',
            to:    els.to?.value   || '',
            act:   els.action?.value || '',
            user:  (els.user?.value || '').trim().toLowerCase(),
            s:     (els.search?.value || '').trim().toLowerCase()
        };

        const filtered = log.filter(ev => {
            if(q.act && ev.action !== q.act) return false;
            if(q.user && (ev.user||'').toLowerCase().indexOf(q.user) === -1) return false;

            // map account info
            const acct = accounts.find(a => a.id === ev.recordId);
            const num  = acct ? String(acct.number) : '';
            const name = acct ? String(acct.name).toLowerCase() : '';

            if(q.s && !(num.includes(q.s) || name.includes(q.s))) return false;

            if(q.from || q.to){
                const t = ev.timestamp ? new Date(ev.timestamp) : null;
                if(!t) return false;
                const from = q.from ? new Date(q.from + 'T00:00:00') : null;
                const to   = q.to   ? new Date(q.to   + 'T23:59:59') : null;
                if(from && t < from) return false;
                if(to   && t > to)   return false;
            }

            return true;
        }).map(ev => ({ ...ev, acct: accounts.find(a=>a.id===ev.recordId) || null }));

        state.rowsAll = filtered.sort((a,b)=>b.timestamp - a.timestamp);
        state.totalPages = Math.max(1, Math.ceil(state.rowsAll.length / state.pageSize));
        render();
    }

    function render(){
        const start = (state.page-1)*state.pageSize;
        const page = state.rowsAll.slice(start, start+state.pageSize);
        state.rowsPage = page;

        els.rows.innerHTML = page.length ? page.map(r => `
      <tr>
        <td>${r.eventId}</td>
        <td>${r.acct ? r.acct.number : '—'}</td>
        <td>${r.acct ? r.acct.name : '—'}</td>
        <td>${r.action}</td>
        <td>${r.user || '—'}</td>
        <td>${new Date(r.timestamp).toLocaleString()}</td>
        <td>${r.comment || ''}</td>
        <td class="text-right">
          <button class="btn btn--secondary btn--small" data-ev="${r.eventId}">View</button>
        </td>
      </tr>
    `).join('') : `<tr class="muted"><td colspan="8" style="text-align:center; padding:16px;">No results</td></tr>`;

        // details pop (simple alert for now)
        $$('#logRows [data-ev]').forEach(btn=>{
            btn.addEventListener('click', () => {
                const id = Number(btn.getAttribute('data-ev'));
                const item = state.rowsAll.find(x=>x.eventId===id);
                alert(
                    `Event #${id} — ${item.action}
User: ${item.user || '—'}
When: ${new Date(item.timestamp).toLocaleString()}
Account: ${item.acct ? item.acct.number + ' • ' + item.acct.name : '—'}

Before:
${JSON.stringify(item.before || {}, null, 2)}

After:
${JSON.stringify(item.after || {}, null, 2)}`
                );
            });
        });

        els.prev.disabled = (state.page<=1);
        els.next.disabled = (state.page>=state.totalPages);
        els.status.textContent = `Page ${state.page} of ${state.totalPages}`;
    }

    function exportCsv(){
        if(!state.rowsAll.length) return;
        const headers = ['Event ID','Account No','Account Name','Action','User','Date/Time','Comment'];
        const rows = state.rowsAll.map(r => [
            r.eventId,
            r.acct ? r.acct.number : '',
            r.acct ? r.acct.name : '',
            r.action,
            r.user || '',
            new Date(r.timestamp).toISOString(),
            (r.comment || '').replace(/"/g,'""')
        ]);
        const csv = [headers, ...rows].map(arr => arr.map(v=>`"${String(v??'').replace(/"/g,'""')}"`).join(',')).join('\n');
        const blob = new Blob([csv], {type:'text/csv;charset=utf-8;'});
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a'); a.href=url; a.download='coa-event-log.csv';
        document.body.appendChild(a); a.click(); a.remove(); URL.revokeObjectURL(url);
    }
})();