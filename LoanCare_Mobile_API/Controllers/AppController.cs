using Business_Services;
using Business_Services.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace LoanCare_Mobile_API.Controllers
{
    //[AuthorizationRequired]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    public class AppController : ApiController
    {
        private readonly IAppService appService;

        public AppController()
        {
            // To do - utilize dependency injection instead of initializing in the constructor
            appService = new AppService(new TokenServices()); 
        }

        [Route("draftdatedelay")]
        public IHttpActionResult GetDraftDateDelay()
        {
            Debug.WriteLine("GetDraftDateDelay() invoked..");

            ResponseModel DelayOptions = appService.GetDraftDateDelay();

            if (DelayOptions != null)
            {
                return Ok(DelayOptions);
            }
            else
            {
                return InternalServerError();
            }
        }
        [Route("HolidayList")]
        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> HolidayList()
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await appService.GetHolidayListAsync(tokenValue);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }


        [Route("help")]
        [HttpGet]
        public IHttpActionResult GetHelpText()
        {
            return Ok(appService.GetHelpText());
        }


        [Route("legaltermsprivacy/{type}")]
        [HttpGet]
        public IHttpActionResult GetLegalTermsPrivacyText(string type)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = appService.GetLegalTermsPrivacy(type);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
        [Route("getAlert/{loan_number}")]
        [HttpGet]
        public IHttpActionResult GetAlertDetails(string loan_number)
        {

            var payment = appService.GetAlertDetailsAsync(loan_number);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("Deletealert/{Alert_id}")]
        [HttpDelete]
        public IHttpActionResult DeleteAlertDetails(string Alert_id)
        {

            var payment = appService.DeleteAlertDetailsAsync(Alert_id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
    }
}

