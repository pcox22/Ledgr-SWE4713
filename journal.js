(function(){ 'use strict';

/* ---------- Utilities ---------- */
const $ = window.$, $$ = window.$$;
const fmtMoney = n => (isNaN(n)?'$0.00': n.toLocaleString(undefined,{style:'currency',currency:'USD'}));
const parseMoney = v => v? Number(v.toString().replace(/[^0-9.-]/g,'')) : 0;
const todayISO = ()=> new Date().toISOString().slice(0,10);
const store = {
  get:(k,d=[])=>{ try{ return JSON.parse(localStorage.getItem(k)) ?? d; }catch{ return d; } },
  set:(k,v)=>localStorage.setItem(k, JSON.stringify(v))
};

const LS_JOURNALS = 'ledgr_journals';

/* Accounts source */
const fallbackAccounts = [
  { number:'101', name:'Cash', normal:'debit' },
  { number:'106', name:'Accounts Receivable', normal:'debit' },
  { number:'201', name:'Accounts Payable', normal:'credit' },
  { number:'301', name:'Owner’s Capital', normal:'credit' },
  { number:'401', name:'Service Revenue', normal:'credit' },
  { number:'501', name:'Rent Expense', normal:'debit' },
];
const getAccounts = ()=> (window.LEDGR_ACCOUNTS?.length ? window.LEDGR_ACCOUNTS : fallbackAccounts);
const accountOptionsHTML = `<option value="">Select an account</option>` +
  getAccounts().map(a => `<option value="${a.number}">${a.number} — ${a.name}</option>`).join("");


/* ---------- Elements ---------- */
const listView = $('#journalListView');
const entryView = $('#journalEntryView');

const rowsEl = $('#journalRows');
const filterForm = $('#filterForm');
const qEl = $('#q'), statusEl = $('#status'), sdEl = $('#startDate'), edEl = $('#endDate');

const btnNew = $('#btnNewEntry');
const backToListBtn = $('#backToList');

const form = $('#journalForm');
const jeRef = $('#jeRef');
const jeDate = $('#jeDate');
const jeDesc = $('#jeDesc');
const jeFiles = $('#jeFiles');

const lineItems = $('#lineItems');
const addLineBtn = $('#addLine');
const totalDebit = $('#totalDebit');
const totalCredit = $('#totalCredit');
const balanceBadge = $('#balanceBadge');
const balanceDiff = $('#balanceDiff');
const balanceState = $('#balanceState');

const saveDraftBtn = $('#saveDraft');
const resetFormBtn = $('#resetForm');
const submitBtn = $('#submitForApproval');

const activityLog = $('#activityLog');

const userMenuBtn = $('#userMenuBtn');
const userMenu = $('#userMenu');
const logoutBtn = $('#logoutBtn');
const activeUserName = $('#activeUserName');

/* ---------- Init ---------- */
function initHeader() {
  // Simple user menu
  userMenuBtn?.addEventListener('click', () => {
    const open = userMenu.hasAttribute('hidden') ? false : true;
    userMenu.hidden = open;
    userMenuBtn.setAttribute('aria-expanded', String(!open));
  });
  document.addEventListener('click', (e) => {
    if (!userMenuBtn.contains(e.target) && !userMenu.contains(e.target)) userMenu.hidden = true;
  });

  // Logout (navigate to login)
  logoutBtn?.addEventListener('click', () => {
    if (typeof window.logout === 'function') {
      window.logout();
    } else {
      window.location.href = 'login.html';
    }
  });

  const name = localStorage.getItem('ledgr_active_user_name');
  if (name) activeUserName.textContent = name;
}

/* ========================= LIST PAGE ========================= */
function initListPage(){
  const rowsEl = $('#journalRows');
  const filterForm = $('#filterForm');
  const qEl = $('#q'), statusEl = $('#status'), sdEl = $('#startDate'), edEl = $('#endDate');

  function statusBadge(s){
    const map = { approved:'badge--success', pending:'badge--warn', rejected:'badge--error', draft:'badge--muted' };
    const cls = map[s] || 'badge--muted';
    const label = s[0].toUpperCase()+s.slice(1);
    return `<span class="badge ${cls}">${label}</span>`;
  }

  function rowHTML(j){
    return `
      <tr>
        <td><a class="linklike" href="journal-entry.html?id=${encodeURIComponent(j.id)}">${j.id}</a></td>
        <td>${j.date}</td>
        <td class="num">${fmtMoney(j.totalDebit)}</td>
        <td class="num">${fmtMoney(j.totalCredit)}</td>
        <td>${statusBadge(j.status)}</td>
        <td class="center"><a class="btn btn--subtle" href="journal-entry.html?id=${encodeURIComponent(j.id)}">View</a></td>
      </tr>
    `;
  }

  function emptyRows(){ return `<tr><td colspan="6" class="center muted">No journal entries found.</td></tr>`; }

  function render(){
    const all = store.get(LS_JOURNALS, []);
    const q = (qEl?.value||'').trim().toLowerCase();
    const st = statusEl?.value || 'all';
    const sd = sdEl?.value ? new Date(sdEl.value) : null;
    const ed = edEl?.value ? new Date(edEl.value) : null;

    const filtered = all.filter(j=>{
      if(st!=='all' && j.status!==st) return false;
      const d = new Date(j.date);
      if(sd && d<sd) return false;
      if(ed && d>ed) return false;
      if(q){
        const hay = [j.id, j.lines?.map(l=>l.accountName).join(' '), fmtMoney(j.totalDebit), fmtMoney(j.totalCredit)].join(' ').toLowerCase();
        if(!hay.includes(q)) return false;
      }
      return true;
    });

    rowsEl.innerHTML = filtered.length ? filtered.map(rowHTML).join('') : emptyRows();
  }

  filterForm?.addEventListener('input', render);
  filterForm?.addEventListener('reset', ()=> setTimeout(render,0));
  render();
}

/* ========================= ENTRY PAGE ========================= */
function initEntryPage(){
  const url = new URL(location.href);
  const idParam = url.searchParams.get('id') || '';

  // elements
  const form = $('#journalForm');
  const jeRef = $('#jeRef');
  const jeDate = $('#jeDate');
  const jeDesc = $('#jeDesc');
  const jeFiles = $('#jeFiles');

  const lineItems = $('#lineItems');
  const addLineBtn = $('#addLine');
  const totalDebit = $('#totalDebit');
  const totalCredit = $('#totalCredit');
  const balanceBadge = $('#balanceBadge');
  const balanceDiff = $('#balanceDiff');
  const balanceState = $('#balanceState');

  // helpers
  function genRef(iso){
    const stamp = (Math.random().toString(36).slice(2,8)).toUpperCase();
    return `JE-${iso}-${stamp}`;
  }
  function populateAccounts(selectEl, selected=''){
    const list = getAccounts();
    selectEl.innerHTML = `<option value="">Select an account</option>` +
      list.map(a => `<option value="${a.number}" ${a.number===selected?'selected':''}>${a.number} — ${a.name}</option>`).join('');
  }

  // show/hide the × only when there are ≥ 2 pairs
  function updateRemoveButtons(){
    const pairs = lineItems.querySelectorAll('.line-pair');
    lineItems.classList.toggle('has-multiple', pairs.length > 1);
    pairs.forEach(p => {
      const xBtn = p.querySelector('.line-item.is-credit .remove');
      if (xBtn) xBtn.style.display = (pairs.length > 1) ? 'inline-flex' : 'none';
    });
  }

  // adds a DEBIT row + an indented CREDIT row (a single "pair")
  function addPair(data = {}){
  const pair = document.createElement('div');
  pair.className = 'line-pair';

  // helper: right-to-left money typing (cents first) with initial value support
  function attachMoneyInput(el, initial){
    const startCents = (typeof initial === 'number')
      ? Math.round(initial * 100)
      : Math.round(parseMoney(el.value || '0') * 100);

    const render = () => {
      const cents = parseInt(el.dataset.cents || '0', 10) || 0;
      const num = cents / 100;
      el.value = num.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      recalc();
    };

    el.dataset.cents = String(isNaN(startCents) ? 0 : startCents);

    el.addEventListener('keydown', (e) => {
      if (e.key >= '0' && e.key <= '9') {
        e.preventDefault();
        el.dataset.cents = String((parseInt(el.dataset.cents || '0', 10) || 0) * 10 + Number(e.key));
        render();
      } else if (e.key === 'Backspace') {
        e.preventDefault();
        el.dataset.cents = String(Math.floor((parseInt(el.dataset.cents || '0', 10) || 0) / 10));
        render();
      } else if (e.key === 'Delete') {
        e.preventDefault();
        el.dataset.cents = '0';
        render();
      } else if (!['Tab','ArrowLeft','ArrowRight','Home','End'].includes(e.key)) {
        e.preventDefault();
      }
    });

    el.addEventListener('paste', (e) => {
      e.preventDefault();
      const digits = (e.clipboardData?.getData('text') || '').replace(/\D/g, '');
      el.dataset.cents = String(parseInt(digits || '0', 10));
      render();
    });

    render();
  }

  // --- Debit row (credit field disabled) ---
  const rowDr = document.createElement('div');
  rowDr.className = 'line-item is-debit';
  rowDr.innerHTML = `
    <select class="account" required aria-label="Debit account"></select>
    <input class="debit num" type="number" step="0.01" min="0" inputmode="decimal" placeholder="0.00" value="${data.debit || ''}">
    <input class="credit num" type="number" step="0.01" min="0" inputmode="decimal" placeholder="0.00" disabled>
    <span></span>
  `;
  populateAccounts($('.account', rowDr), data.debitAccount || '');
  attachMoneyInput($('.debit', rowDr), data.debit);

  // --- Credit row (debit field disabled) ---
  const rowCr = document.createElement('div');
  rowCr.className = 'line-item is-credit';
  rowCr.innerHTML = `
    <select class="account" required aria-label="Credit account"></select>
    <input class="debit num" type="number" step="0.01" min="0" inputmode="decimal" placeholder="0.00" disabled>
    <input class="credit num" type="number" step="0.01" min="0" inputmode="decimal" placeholder="0.00" value="${data.credit || ''}">
    <button type="button" class="remove" title="Remove">✕</button>
  `;
  populateAccounts($('.account', rowCr), data.creditAccount || '');
  attachMoneyInput($('.credit', rowCr), data.credit);

  // interactions
  $('.debit', rowDr).addEventListener('input', recalc);
  $('.credit', rowCr).addEventListener('input', recalc);
  $('.account', rowDr).addEventListener('change', () => { enforceUnique(); disableAlreadyUsed(); });
  $('.account', rowCr).addEventListener('change', () => { enforceUnique(); disableAlreadyUsed(); });
  $('.remove', rowCr).addEventListener('click', () => {
    const pairs = lineItems.querySelectorAll('.line-pair');
    if (pairs.length <= 1) return;
    pair.remove();
    updateRemoveButtons();
    enforceUnique();
    disableAlreadyUsed();
    recalc();
  });

  pair.appendChild(rowDr);
  pair.appendChild(rowCr);
  lineItems.appendChild(pair);

  updateRemoveButtons();
  enforceUnique();
  disableAlreadyUsed();
  recalc();
}


  function enforceUnique(){
    const vals = $$('.account', lineItems).map(s=>s.value).filter(Boolean);
    const dupes = vals.filter((v,i)=> vals.indexOf(v)!==i);
    $$('.account', lineItems).forEach(s=>{
      s.setCustomValidity(dupes.includes(s.value) ? 'Duplicate account.' : '');
    });
  }

  // --- disable accounts already selected elsewhere ---
function disableAlreadyUsed(){
  const allSelects = $$('.account', lineItems);
  const used = new Set(allSelects.map(s => s.value).filter(Boolean));
  allSelects.forEach(sel => {
    const current = sel.value;
    $$('option', sel).forEach(opt => {
      if (!opt.value) return;
      opt.disabled = (opt.value !== current) && used.has(opt.value);
    });
  });
}

  function recalc(){
    let td=0, tc=0;
    $$('.line-item', lineItems).forEach(r=>{
      td += parseMoney($('.debit',r).value);
      tc += parseMoney($('.credit',r).value);
    });
    totalDebit.textContent = fmtMoney(td);
    totalCredit.textContent = fmtMoney(tc);
    const diff = Math.abs(td-tc);
    balanceDiff.textContent = `(Difference: ${fmtMoney(diff)})`;
    const ok = td>0 && tc>0 && diff===0;
    balanceState.classList.toggle('balance--ok', ok);
    balanceState.classList.toggle('balance--notok', !ok);
    balanceBadge.textContent = ok ? 'Balanced' : 'Not Balanced';
  }

  function log(msg){
    const box = $('#activityLog');
    if(!box) return;
    const time = new Date().toLocaleTimeString([], {hour:'2-digit', minute:'2-digit'});
    const el = document.createElement('div');
    el.className = 'item';
    el.textContent = `${time} — ${msg}`;
    box.prepend(el);
  }

  function collect(status){
    const lines = $$('.line-item', lineItems).map(r=>{
      const num = $('.account',r).value;
      const acct = getAccounts().find(a=>a.number===num);
      return {
        accountNumber: num,
        accountName: acct ? acct.name : '',
        debit:  parseMoney($('.debit',r).value),
        credit: parseMoney($('.credit',r).value)
      };
    }).filter(l=>l.accountNumber);

    return {
      id: jeRef.textContent,
      date: jeDate.value || todayISO(),
      description: jeDesc.value?.trim()||'',
      status,
      lines,
      totalDebit: lines.reduce((s,l)=>s+l.debit,0),
      totalCredit: lines.reduce((s,l)=>s+l.credit,0),
      attachments: Array.from(jeFiles?.files||[]).map(f=>({name:f.name,type:f.type,size:f.size})),
      createdBy: localStorage.getItem('ledgr_active_user_name') || 'Admin',
      createdAt: new Date().toISOString()
    };
  }

  function save(status){
    const entry = collect(status);

    // validation
    if(!entry.lines.length){ alert('Add at least one entry line.'); return; }
    const nums = entry.lines.map(l=>l.accountNumber);
    const hasDupes = nums.some((v,i)=> nums.indexOf(v)!==i);
    if(hasDupes){ alert('Duplicate accounts are not allowed.'); return; }
    if(status!=='draft'){
      if(Math.abs(entry.totalDebit-entry.totalCredit)!==0 || entry.totalDebit===0){
        alert('Debits must equal credits (and be greater than $0.00) before submitting.'); return;
      }
    }

    let all = store.get(LS_JOURNALS, []);
    const idx = all.findIndex(x=>x.id===entry.id);
    if(idx>=0) all[idx]=entry; else all.unshift(entry);
    store.set(LS_JOURNALS, all);

    log(status==='draft'?'Draft saved':'Submitted for approval');
    if(status!=='draft') location.href = 'journal.html';
  }

  function loadExisting(id){
    const all = store.get(LS_JOURNALS, []);
    const j = all.find(x=>x.id===id);
    if(!j) return false;

    jeRef.textContent = j.id;
    jeDate.value = j.date || todayISO();
    jeDesc.value = j.description || '';

    lineItems.innerHTML = '';

    // rebuild stored single-line items into debit/credit pairs
    const debits  = (j.lines || []).filter(l => l.debit  > 0);
    const credits = (j.lines || []).filter(l => l.credit > 0);
    const maxLen = Math.max(debits.length, credits.length) || 1;

    for (let i = 0; i < maxLen; i++) {
      addPair({
        debitAccount:  debits[i]?.accountNumber  || '',
        debit:         debits[i]?.debit          || '',
        creditAccount: credits[i]?.accountNumber || '',
        credit:        credits[i]?.credit        || ''
      });
    }
    recalc();
    log(`Opened ${j.id} (${j.status})`);
    return true;
  }

  // lock the entry UI for anything that's not a draft
function setReadOnlyIfLocked(status){
  if (status === 'draft') return;

  const form = $('#journalForm');
  const lineItems = $('#lineItems');

  // visual flag (optional if you add CSS)
  form.classList.add('is-readonly');

  // disable all fields
  form.querySelectorAll('input, select, textarea').forEach(el => {
    el.disabled = true;
  });

  // hide actions that would change data
  $('#addLine')?.setAttribute('hidden','');
  $('#saveDraft')?.setAttribute('hidden','');
  $('#resetForm')?.setAttribute('hidden','');
  $('#submitForApproval')?.setAttribute('hidden','');

  // hide per-line remove buttons
  $$('.line-item .remove', lineItems).forEach(b => b.style.display = 'none');
}

  // ---- init ----
  jeDate.value = todayISO();
  if (idParam){
    if(!loadExisting(idParam)){
      jeRef.textContent = idParam;
      addPair();
      recalc();
    }
  } else {
    const ref = genRef(todayISO());
    jeRef.textContent = ref;
    addPair();
  }

  $('#saveDraft')?.addEventListener('click', ()=> save('draft'));
  $('#resetForm')?.addEventListener('click', ()=> setTimeout(()=>{
    lineItems.innerHTML=''; addPair(); recalc(); log('Form reset');
  },0));
  $('#addLine')?.addEventListener('click', ()=> addPair());
  $('#journalForm')?.addEventListener('submit', (e)=>{ e.preventDefault(); save('pending'); });
}

/* ========================= BOOT ========================= */
document.addEventListener('DOMContentLoaded', ()=>{
  initHeader();
  const page = document.body.getAttribute('data-page');
  if(page==='list') initListPage();
  if(page==='entry') initEntryPage();
});

})();
