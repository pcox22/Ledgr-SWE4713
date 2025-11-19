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
    
    public void LedgerCall(string Username, string AccountNumber)
    {
        _nav.NavigateTo($"/ledger/{Username}/{AccountNumber}");
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

    public void JournalEntryAdjustManager(string Username)
    {
        _nav.NavigateTo($"/adjustJournalEntryManager/{Username}");
    }

    public void Reports(string Username)
    {
        _nav.NavigateTo($"/managerReports/{Username}");
    }

    public void TrialBalanceCall(string Username)
    {
        _nav.NavigateTo($"/trialBalance/{Username}");
    }

    public void IncomeStatementCall(string Username)
    {
        _nav.NavigateTo($"/incomeStatement/{Username}");
    }

    public void BalanceSheetCall(string Username)
    {
        _nav.NavigateTo($"/balanceSheet/{Username}");
    }

    public void RetainedEarningsCall(string Username)
    {
        _nav.NavigateTo($"/retainedEarnings/{Username}");
    }

    public void AdjustJournal(string Username)
    {
        _nav.NavigateTo($"/JournalAdjustments/{Username}");
    }
    public void AdjustJournalEntry(string Username)
    {
        _nav.NavigateTo($"/adjustJournalEntry/{Username}");
    }
    public void JournalEntry(string Username)
    {
        _nav.NavigateTo($"/journalEntry/{Username}");
    }

    public void HelpPage(string Username)
    {
        _nav.NavigateTo($"/help/{Username}");
    }
    public void LogOut(string Username)
    {
        _nav.NavigateTo("/");
    }
}