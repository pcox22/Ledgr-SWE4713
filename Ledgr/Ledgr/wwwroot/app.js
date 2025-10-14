const $  = (s, r=document)=>r.querySelector(s);
const $$ = (s, r=document)=>[...r.querySelectorAll(s)];

// Alerts
function showAlert(boxId, type, msg){
    const box = document.getElementById(boxId);
    const txt = document.getElementById(boxId + 'Text');
    if(!box||!txt) return;
    box.classList.remove('is-hidden','-error','-warning','-info','-success');
    box.classList.add(`-${type}`);
    txt.textContent = msg;
}
function hideAlert(boxId){ document.getElementById(boxId)?.classList.add('is-hidden'); }

// Email check
const isEmail = v => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test((v||'').trim());

// Password rules (Sprint 1)
function goodPassword(pw){
    if(!pw) return false;
    const starts = /^[A-Za-z]/.test(pw);
    const len = pw.length >= 8;
    const hasL = /[A-Za-z]/.test(pw);
    const hasD = /\d/.test(pw);
    const hasS = /[^A-Za-z0-9]/.test(pw);
    return starts && len && hasL && hasD && hasS;
}

// ROUTE HELPERS
function onSubmit(formId, cb){
    const form = document.getElementById(formId);
    if(!form) return;
    form.addEventListener('submit', e=>{ e.preventDefault(); cb(e, form); });
}

// LOGIN
(function(){
    const form = $('#loginForm');
    if(!form) return;

    const pwd = $('#password');
    const tog = $('#togglePwd');
    tog?.addEventListener('click', ()=>{
        const open = pwd.type === 'password';
        pwd.type = open ? 'text' : 'password';
        tog.setAttribute('aria-label', open ? 'Hide password' : 'Show password');
        const img = tog.querySelector('img');
        if(img) img.src = open ? 'assets/icons/hidden.svg' : 'assets/icons/visible.svg';
    });

    $('#btnClear')?.addEventListener('click', ()=>{ form.reset(); hideAlert('loginAlert'); $('#email')?.focus(); });

    onSubmit('loginForm', ()=>{
        const email = $('#email')?.value;
        const pass  = $('#password')?.value;
        if(!isEmail(email)) return showAlert('loginAlert','error','Enter a valid email.');
        if(!goodPassword(pass)) return showAlert('loginAlert','error','Password does not meet the rules.');
        // Fake success
        showAlert('loginAlert','success','Signed in. Redirecting to User Management…');
        setTimeout(()=> location.href='admin-user-management.html', 600);
    });
})();

// CREATE USER
(function(){
    const form = $('#createForm');
    if(!form) return;
    const first=$('#firstName'), last=$('#lastName'), dob=$('#dob'), user=$('#username'), email=$('#email');
    const slug = s => (s||'').toLowerCase().replace(/[^a-z]/g,'');
    const mmYy = () => /^\d{2}\/\d{2}\/\d{4}$/.test(dob?.value||'') ? dob.value.slice(0,2)+dob.value.slice(8,10) : '';

    function genUser(){
        const handle = (slug(first?.value).slice(0,1) + slug(last?.value) + mmYy()) || '';
        if(user) user.value = handle;
    }
    first?.addEventListener('input', genUser);
    last?.addEventListener('input', genUser);
    dob?.addEventListener('input', ()=>{
        let v = dob.value.replace(/\D/g,'').slice(0,8);
        if (v.length >= 5)      dob.value = `${v.slice(0,2)}/${v.slice(2,4)}/${v.slice(4,8)}`;
        else if (v.length >=3 ) dob.value = `${v.slice(0,2)}/${v.slice(2,4)}`;
        else                    dob.value = v;
        genUser();
    });
    dob?.addEventListener('blur', ()=>{ if(!/^\d{2}\/\d{2}\/\d{4}$/.test(dob.value)) dob.value=''; genUser(); });

    $('#btnClearCreate')?.addEventListener('click', ()=>{ form.reset(); hideAlert('createAlert'); if(user){user.value=''; user.placeholder='jdoe1025';} first?.focus(); });

    onSubmit('createForm', ()=>{
        if(!isEmail(email?.value)) return showAlert('createAlert','error','Enter a valid email.');
        showAlert('createAlert','success','User submitted. Awaiting approval.');
    });
})();

// FORGOT PASSWORD — IDENTIFY
(function(){
    const form = $('#fpFormIdentify');
    if (!form) return;

    // Clear button
    const clr = document.getElementById('btnFpClear');
    if (clr) {
        clr.addEventListener('click', () => {
            form.reset();
            hideAlert('fpAlert1');           // hide the red banner
            document.getElementById('fpUserId')?.focus();
        });
    }

    onSubmit('fpFormIdentify', ()=>{
        const email = $('#fpEmail')?.value;
        if (!isEmail(email)) return showAlert('fpAlert1','error','Enter a valid email.');
        showAlert('fpAlert1','success','User has been found. Redirecting to security questions.');
        setTimeout(()=> location.href='forgot-password-verify.html', 600);
    });
})();



// FORGOT PASSWORD — VERIFY (Security Questions)
(function(){
    const form = $('#fpFormVerify');
    if (!form) return;

    // Clear button
    $('#btnSecQClear')?.addEventListener('click', () => {
        form.reset();
        hideAlert('fpAlert2');
        $('#q1')?.focus();
    });

    onSubmit('fpFormVerify', () => {
        const a1 = $('#q1')?.value?.trim();
        const a2 = $('#q2')?.value?.trim();
        const a3 = $('#q3')?.value?.trim();

        // Require all three answers
        if (!a1 || !a2 || !a3) {
            return showAlert('fpAlert2', 'error', 'Please answer all security questions.');
        }

        // TODO: replace with backend verification
        // For Sprint 1, treat non-empty answers as success:
        showAlert('fpAlert2', 'success', 'Identity verified.');
        setTimeout(() => location.href = 'forgot-password-reset.html', 600);
    });
})();


// FORGOT PASSWORD — RESET
(function () {
    const form = $('#fpFormReset');
    if (!form) return;

    const pwd  = $('#newPwd');
    const pwd2 = $('#newPwd2');

    // Toggle show/hide for New Password
    $('#toggleNewPwd')?.addEventListener('click', () => {
        const open = pwd.type === 'password';
        pwd.type = open ? 'text' : 'password';
        $('#toggleNewPwd').setAttribute('aria-label', open ? 'Hide password' : 'Show password');
        const img = $('#toggleNewPwd img');
        if (img) img.src = open ? 'assets/icons/hidden.svg' : 'assets/icons/visible.svg';
    });

    // Placeholder: replace with real server-side check
    async function passwordWasUsedBefore(newPassword) {
        // Example:
        // const res = await fetch('/api/password-history/check', {
        //   method: 'POST',
        //   headers: { 'Content-Type': 'application/json' },
        //   body: JSON.stringify({ password: newPassword })
        // });
        // const data = await res.json();
        // return data?.isReused === true;
        return false; // <- default to "not reused" until wired up
    }

    onSubmit('fpFormReset', async () => {
        if (!goodPassword(pwd?.value)) {
            return showAlert('fpAlert3', 'error', 'Password does not meet security requirements. Please try again.');
        }

        if (pwd?.value !== pwd2?.value) {
            return showAlert('fpAlert3', 'error', 'Passwords do not match.');
        }

        // New: disallow reusing an old password
        if (await passwordWasUsedBefore(pwd?.value)) {
            return showAlert('fpAlert3', 'error', 'You cannot reuse a previous password. Please choose a new one.');
        }

        showAlert('fpAlert3', 'success', 'Password updated. You can sign in now.');
        setTimeout(() => (location.href = 'login.html'), 700);
    });
})();


// ADMIN — USER MANAGEMENT (desktop layout + overlays)
(function(){
    const table = $('#usersTable');
    if (!table) return;
    const tbody = table.querySelector('tbody');
    const search = $('#userSearch');

    // Sample data
    let users = [
        {username:'alovelace', first:'Ada', last:'Lovelace', role:'Admin',      status:'Active'},
        {username:'ghopper',   first:'Grace', last:'Hopper', role:'Manager',    status:'Active'},
        {username:'kjohnson',  first:'Katherine', last:'Johnson', role:'Accountant', status:'Inactive'},
        {username:'mmirzakhani',first:'Maryam', last:'Mirzakhani', role:'Accountant', status:'Active'},
        {username:'bhamilton', first:'Barbara', last:'Hamilton', role:'Manager', status:'Inactive'},
    ];

    function render(list){
        tbody.innerHTML = list.map(u => `
      <tr>
        <td><input type="checkbox" aria-label="Select ${u.username}"></td>
        <td>${u.username}</td>
        <td>${u.first} ${u.last}</td>
        <td>${u.role}</td>
        <td><span class="badge">${u.status}</span></td>
        <td>
          <button class="link" data-act="edit" data-uid="${u.username}">Edit</button> ·
          <button class="link" data-act="email" data-uid="${u.username}">Email</button> ·
          <button class="link" data-act="${u.status==='Active'?'deactivate':'activate'}" data-uid="${u.username}">
            ${u.status==='Active'?'Deactivate':'Activate'}
          </button>
        </td>
      </tr>
    `).join('');
        $('#tableCount').textContent = `Showing ${list.length} of ${users.length} users`;
    }
    render(users);

    // Search
    search?.addEventListener('input', e=>{
        const q = e.target.value.toLowerCase();
        const filtered = users.filter(u => Object.values(u).join(' ').toLowerCase().includes(q));
        render(filtered);
    });

    // Modal helpers
    const backdrop = $('#backdrop');
    const mod = id => document.getElementById(id);
    const open = id => { backdrop.classList.add('show'); mod(id).classList.add('show'); };
    const closeAll = () => { backdrop.classList.remove('show'); $$('.modal').forEach(m=>m.classList.remove('show')); };
    backdrop?.addEventListener('click', closeAll);
    document.addEventListener('click', e=>{ if(e.target.matches('[data-close]')) closeAll(); });

    // Open overlays
    $('#btnOpenQueue')?.addEventListener('click', ()=> open('modalQueue'));
    $('#btnOpenReports')?.addEventListener('click', ()=> open('modalReports'));
    $('#btnOpenCreate')?.addEventListener('click', ()=> open('modalCreate'));

    // Reports (CSV)
    $('#btnCsvUsers')?.addEventListener('click', ()=>{
        $('#repAlert')?.classList.remove('is-hidden'); $('#repAlertText').textContent='Generating users.csv…';
        // TODO: replace with real CSV download
    });
    $('#btnCsvPw')?.addEventListener('click', ()=>{
        $('#repAlert')?.classList.remove('is-hidden'); $('#repAlertText').textContent='Generating passwords.csv…';
    });

    // Create overlay: username generation from First/Last + DOB (MMyy)
    const ovFirst = $('#ovFirst'), ovLast = $('#ovLast'), ovEmail = $('#ovEmail');
    const ovDob = $('#ovDob'), ovUser = $('#ovUser'), ovRole = $('#ovRole'), ovStatus = $('#ovStatus');

    const slug = s => (s||'').toLowerCase().replace(/[^a-z]/g,'');
    const mmYy = () => /^\d{2}\/\d{2}\/\d{4}$/.test(ovDob?.value||'') ? ovDob.value.slice(0,2)+ovDob.value.slice(8,10) : '';

    function genOvUser(){
        ovUser.value = (slug(ovFirst?.value).slice(0,1) + slug(ovLast?.value) + mmYy()) || '';
    }
    ovFirst?.addEventListener('input', genOvUser);
    ovLast?.addEventListener('input', genOvUser);
    ovDob?.addEventListener('input', ()=>{
        let v = ovDob.value.replace(/\D/g,'').slice(0,8);
        if (v.length >= 5)      ovDob.value = `${v.slice(0,2)}/${v.slice(2,4)}/${v.slice(4,8)}`;
        else if (v.length >=3 ) ovDob.value = `${v.slice(0,2)}/${v.slice(2,4)}`;
        else                    ovDob.value = v;
        genOvUser();
    });
    ovDob?.addEventListener('blur', ()=>{ if(!/^\d{2}\/\d{2}\/\d{4}$/.test(ovDob.value)) ovDob.value=''; genOvUser(); });

    $('#ovClear')?.addEventListener('click', ()=>{
        $('#createOverlayForm')?.reset(); ovUser.value=''; $('#createInline')?.classList.add('is-hidden'); ovFirst?.focus();
    });

    $('#createOverlayForm')?.addEventListener('submit', e=>{
        e.preventDefault();
        const emailOk = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(ovEmail?.value||'');
        if(!emailOk || !ovUser.value){
            $('#createInline')?.classList.remove('is-hidden'); $('#createInlineText').textContent='Please complete all fields with a valid email and DOB.';
            return;
        }
        users.unshift({username: ovUser.value, first: ovFirst.value, last: ovLast.value, role: ovRole.value, status: ovStatus.value});
        render(users);
        $('#createInline')?.classList.remove('is-hidden'); $('#createInlineText').textContent='User has been successfully added.';
        setTimeout(closeAll, 700);
    });

    // Approval Queue overlay
    const qTbody = $('#approvalTable')?.querySelector('tbody');
    const pending = [
        {name:'John Doe', email:'jdoe@ledgr.com', role:'Accountant', requested:'2025-09-28'},
        {name:'Jane Smith', email:'jsmith@ledgr.com', role:'Manager', requested:'2025-09-29'}
    ];
    function renderQueue(list){
        if(!qTbody) return;
        qTbody.innerHTML = list.map(p=>`
      <tr>
        <td>${p.name}</td>
        <td>${p.email}</td>
        <td>${p.role}</td>
        <td>${p.requested}</td>
        <td>
          <button class="btn btn--small" data-approve="${p.email}">Approve</button>
          <button class="btn btn--small btn--secondary" data-reject="${p.email}">Reject</button>
        </td>
      </tr>
    `).join('');
    }
    renderQueue(pending);

    qTbody?.addEventListener('click', e=>{
        const a = e.target.closest('[data-approve]'); const r = e.target.closest('[data-reject]');
        if(a){ const email=a.getAttribute('data-approve'); for(let i=pending.length-1;i>=0;i--) if(pending[i].email==email) pending.splice(i,1); renderQueue(pending); }
        if(r){ const email=r.getAttribute('data-reject'); for(let i=pending.length-1;i>=0;i--) if(pending[i].email==email) pending.splice(i,1); renderQueue(pending); }
    });

    // Row actions: Edit / Email / Activate|Deactivate (stubs)
    tbody.addEventListener('click', e=>{
        const btn = e.target.closest('button[data-act]'); if(!btn) return;
        const uid = btn.dataset.uid;
        const u = users.find(x=>x.username===uid); if(!u) return;

        if(btn.dataset.act==='edit'){ open('modalCreate'); // reuse create modal to keep Sprint-1 simple
            ovFirst.value=u.first; ovLast.value=u.last; ovRole.value=u.role; ovStatus.value=u.status; ovEmail.value=`${u.username}@ledgr.com`; ovDob.value=''; ovUser.value=u.username;
            return;
        }
        if(btn.dataset.act==='email'){ open('modalReports'); return; } // placeholder
        u.status = (btn.dataset.act==='activate') ? 'Active' : 'Inactive';
        render(users);
    });

})();

// Example role: 'Admin' | 'Manager' | 'Accountant'
window.currentUserRole = window.currentUserRole || 'Admin';