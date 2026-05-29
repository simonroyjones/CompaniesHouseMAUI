using CompanyLookupMAUI.Models;
using CompanyLookupMAUI.Services;

namespace CompanyLookupMAUI;

public partial class MainPage : ContentPage
{
    private readonly CompaniesHouseService _companiesHouseService = new();

    public MainPage()
    {
        InitializeComponent();

        SearchTypePicker.SelectedIndex = 0;
    }

    private void SearchTypePicker_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        ClearResults();

        if (SearchTypePicker.SelectedIndex == 0)
        {
            SearchEntry.Placeholder = "Enter company number";
        }
        else
        {
            SearchEntry.Placeholder = "Enter company name";
        }
    }

    private async void SearchButton_OnClicked(object? sender, EventArgs e)
    {
        ClearResults();

        var searchText = SearchEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            StatusLabel.Text = "Please enter a company number or company name.";
            return;
        }

        try
        {
            SetLoading(true);

            if (SearchTypePicker.SelectedIndex == 0)
            {
                var company = await _companiesHouseService.GetCompanyByNumberAsync(searchText);

                if (company == null)
                {
                    StatusLabel.Text = "No company found.";
                    return;
                }

                ShowCompanyProfile(company);
            }
            else
            {
                var results = await _companiesHouseService.SearchCompaniesByNameAsync(searchText);

                if (results.Count == 0)
                {
                    StatusLabel.Text = "No companies found.";
                    return;
                }

                SearchResultsCollectionView.ItemsSource = results;
                SearchResultsFrame.IsVisible = true;
                StatusLabel.Text = $"{results.Count} result(s) found. Select one to view full details.";
            }
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error: {ex.Message}";
        }
        finally
        {
            SetLoading(false);
        }
    }

    private async void SearchResultsCollectionView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var selectedCompany = e.CurrentSelection.FirstOrDefault() as CompanySearchItem;

        if (selectedCompany == null || string.IsNullOrWhiteSpace(selectedCompany.CompanyNumber))
            return;

        try
        {
            SetLoading(true);

            var company = await _companiesHouseService.GetCompanyByNumberAsync(selectedCompany.CompanyNumber);

            if (company != null)
            {
                ShowCompanyProfile(company);
            }
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error loading company profile: {ex.Message}";
        }
        finally
        {
            SetLoading(false);

            SearchResultsCollectionView.SelectedItem = null;
        }
    }

    private void ShowCompanyProfile(CompanyProfile company)
    {
        CompanyNameLabel.Text = $"Name: {company.CompanyName}";
        CompanyNumberLabel.Text = $"Number: {company.CompanyNumber}";
        CompanyStatusLabel.Text = $"Status: {company.CompanyStatus}";
        CompanyTypeLabel.Text = $"Type: {company.CompanyType}";
        DateCreatedLabel.Text = $"Date Created: {company.DateOfCreation}";
        AddressLabel.Text = $"Address: {company.RegisteredOfficeAddress}";

        CompanyProfileFrame.IsVisible = true;
        StatusLabel.Text = "Company details loaded.";
    }

    private void ClearResults()
    {
        StatusLabel.Text = "";
        CompanyProfileFrame.IsVisible = false;
        SearchResultsFrame.IsVisible = false;
        SearchResultsCollectionView.ItemsSource = null;
    }

    private void SetLoading(bool isLoading)
    {
        LoadingIndicator.IsVisible = isLoading;
        LoadingIndicator.IsRunning = isLoading;
    }
}