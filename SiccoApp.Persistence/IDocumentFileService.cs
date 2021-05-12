using System.Threading.Tasks;
using System.Web;

namespace SiccoApp.Persistence
{
    public interface IDocumentFileService
    {
        void CreateAndConfigureAsync();
        Task<string> UploadDocumentFileAsync(HttpPostedFileBase documentFileToUpload);
        Task<string> UploadDocumentFileAsync(HttpPostedFileBase documentFileToUpload, string fileName);
        Task DeleteDocumentFileAsync(string documentFileName);
    }
}
