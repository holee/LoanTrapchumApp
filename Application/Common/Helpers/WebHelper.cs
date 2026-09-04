namespace Application.Helpers;

public class WebHelper
{
    //private readonly HttpClient httpClient;

    //public WebHelper(HttpClient httpClient)
    //{
    //    this.httpClient = httpClient;
    //}

    //public async Task<List<T>> ParseHtmlToTable<T>(string tableName)
    //{ 
    //    var response = await httpClient.GetStringAsync("https://www.nbc.gov.kh/english/economic_research/exchange_rate.php");

    //    HtmlDocument html = new HtmlDocument();

    //    html.LoadHtml(response);

    //    var table=html.DocumentNode.Descendants("table")
    //                    .Where(node =>node.GetAttributeValue("class","").Equals("tbl-responsive"))
    //                        .FirstOrDefault();   

    //    return new List<T>();   
    //}

}
