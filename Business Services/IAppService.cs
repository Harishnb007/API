using Business_Services.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services
{
    public interface IAppService
    {
        ResponseModel GetDraftDateDelay();
        ResponseModel GetLegalTermsPrivacy(string type);
        ResponseModel GetHelpText();
        Task<ResponseModel> GetHolidayListAsync(string MobileToken);
        ResponseModel GetAlertDetailsAsync(string loanNumber);
        ResponseModel DeleteAlertDetailsAsync(string Alert_id);

    }
}
