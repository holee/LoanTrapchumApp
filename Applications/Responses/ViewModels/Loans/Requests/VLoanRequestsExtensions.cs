namespace Applications.Responses.ViewModels.Loans.Requests;

public static class VLoanRequestsExtensions
{
    public static List<VLoanRequestGuarantors> ToLoanRequestGuarantors(this VLoanRequests req)
    {
        return req.Guarantors.ToList() ?? [];
    }

    public static List<VLoanRequestCollateralLines> ToLoanRequestCollaterals(this VLoanRequests req)
    {
        return req.Collaterals.ToList() ?? [];
    }

    public static List<VLoanRequestsDocuments> ToLoanRequestDocuments(this VLoanRequests req)
    {
        return req.Documents.ToList() ?? [];
    }

}
