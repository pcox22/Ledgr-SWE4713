namespace LedgrLogic;
using Microsoft.AspNetCore.Components;

public class Navigation
{
    private readonly NavigationManager _nav;

    public Navigation(NavigationManager nav)
    {
        _nav = nav;
    }
    
    // Nav Buttons - Linked to side bar
    public void Home(string Username)
    {
        _nav.NavigateTo($"/dashboardN/{Username}");
    }
    public void CoAListCall(string Username)
    {
        _nav.NavigateTo($"/CoAList/{Username}");
    }
    
    public void LedgerCall(string Username)
    {
        _nav.NavigateTo($"/ledger/{Username}");
    }
    
    public void CoAForm(string Username)
    {
        _nav.NavigateTo($"/CoAForm/{Username}");
    }
    public void CoAEventLog(string Username)
    {
        _nav.NavigateTo($"/CoAEventLog/{Username}");
    }

    public void ManageUser(string Username)
    {
        _nav.NavigateTo($"/manageUsers/{Username}");
    }
    public void PasswordReport(string Username)
    {
        _nav.NavigateTo($"/pwReport/{Username}");
    }

    public void JournalCall(string Username)
    {
        _nav.NavigateTo($"/journal/{Username}");
    }
    public void LogOut(string Username)
    {
        _nav.NavigateTo("/");
    }
}