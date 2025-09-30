/* =========================================================
   LEDGR prototype JS (minimal; backend-ready hook points)
   ========================================================= */

/* ===== Login page: show/hide password (click only) ===== */
const pwdInput = document.getElementById('password');
const togglePwd = document.getElementById('togglePwd');
if (pwdInput && togglePwd) {
  let visible = false;
  togglePwd.addEventListener('click', () => {
    visible = !visible;
    pwdInput.type = visible ? 'text' : 'password';
    const img = togglePwd.querySelector('img');
    // Uses icons/visible.svg and icons/hidden.svg
    if (img) img.src = visible ? 'assets/icons/hidden.svg' : 'assets/icons/visible.svg';
    togglePwd.setAttribute('aria-label', visible ? 'Hide password' : 'Show password');
    pwdInput.focus();
  });
}

/* ===== Login: demo submit (replace with backend) ===== */
const form = document.getElementById('loginForm');
if (form) {
  form.addEventListener('submit', (e) => {
    e.preventDefault();
    const email = (document.getElementById('email') || {}).value?.trim();
    const password = (pwdInput || {}).value;

    // TODO backend: POST /api/auth/login {email,password}
    const demoOK = email === 'admin@ledgr.com' && password === 'Abcd123!';
    if (demoOK) {
      showAlert('success', 'Signed in successfully.');
      setTimeout(() => { window.location.href = 'admin-user-management.html'; }, 600);
    } else {
      const msg = email ? 'Incorrect password. Please try again.' : 'User not found. Try creating a new account.';
      showAlert('error', msg);
    }
  });

  const btnClear = document.getElementById('btnClear');
  if (btnClear) btnClear.addEventListener('click', hideAlert);
}

/* ===== Inline alert utilities (Login page) ===== */
function showAlert(kind, text) {
  const el = document.getElementById('loginAlert');
  const tx = document.getElementById('loginAlertText');
  if (!el || !tx) return;
  el.classList.remove('is-hidden', '-error', '-warning', '-info', '-success');
  el.classList.add(`-${kind}`);
  tx.textContent = text || '';
  el.setAttribute('role', 'alert');
}
function hideAlert() {
  const el = document.getElementById('loginAlert');
  if (el) el.classList.add('is-hidden');
}

/* =========================================================
   Reusable helpers for other pages
   ========================================================= */

/* Attach click-to-toggle eye to any password field */
function attachPasswordToggle(inputId, btnId){
  const input = document.getElementById(inputId);
  const btn = document.getElementById(btnId);
  if(!(input && btn)) return;
  let visible = false;
  btn.addEventListener('click', ()=>{
    visible = !visible;
    input.type = visible ? 'text' : 'password';
    const img = btn.querySelector('img');
    if(img) img.src = visible ? 'assets/icons/hidden.svg' : 'assets/icons/visible.svg';
    btn.setAttribute('aria-label', visible ? 'Hide password' : 'Show password');
    input.focus();
  });
}

/* Page-local inline alert */
function showInline(containerId, kind, text){
  const el = document.getElementById(containerId);
  const tx = document.getElementById(containerId + 'Text');
  if(!el || !tx) return;
  el.classList.remove('is-hidden','-error','-warning','-info','-success');
  el.classList.add(`-${kind}`);
  tx.textContent = text || '';
  el.setAttribute('role','alert');
}

/* Simple form hook for non-login pages (backend can replace) */
function hookSimpleSubmit(formId, alertId, alertTextId, onOk){
  const form = document.getElementById(formId);
  if(!form) return;
  form.addEventListener('submit', (e)=>{
    e.preventDefault();
    // TODO backend: call API and show errors with showInline(alertId,'error','...')
    if(typeof onOk === 'function') onOk();
  });
}

/* OTP inputs: auto-advance + backspace */
function setupOTP(selector){
  const cells = Array.from(document.querySelectorAll(selector));
  cells.forEach((cell, idx)=>{
    cell.addEventListener('input', ()=>{
      cell.value = cell.value.replace(/[^0-9]/g,'').slice(0,1);
      if(cell.value && cells[idx+1]) cells[idx+1].focus();
    });
    cell.addEventListener('keydown', (e)=>{
      if(e.key === 'Backspace' && !cell.value && cells[idx-1]) cells[idx-1].focus();
    });
  });
}
function readOTP(selector){
  return Array.from(document.querySelectorAll(selector)).map(c=>c.value).join('');
}

/* Expose helpers for inline scripts */
window.attachPasswordToggle = attachPasswordToggle;
window.showInline = showInline;
window.hookSimpleSubmit = hookSimpleSubmit;
window.setupOTP = setupOTP;
window.readOTP = readOTP;

