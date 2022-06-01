using System.Threading.Tasks;
using RestEase;
using SongsParser.Dtos;

namespace SongsParser.ApiInterfaces
{ 
    public interface IBillboardChartsApi
    {
        [Get("pmc-ajax/charts-fetch-all-chart/selected_category-{category}/chart_type-weekly/")]
        Task<HtmlDto> GetChartsHtmlAsync([Path] string category);
    }

}
