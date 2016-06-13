using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using POD.Portal;

namespace POD.Portal.Controllers.WebApi
{
    public class TranslationController : ApiController
    {
        [Route("api/translation")]
        [HttpGet]
        public IHttpActionResult GetAllTranslations(string lang)
        {
            var resourceObject = new JObject();

            var resourceSet = Resource.ResourceManager.GetResourceSet(new CultureInfo(lang), true, true);
            IDictionaryEnumerator enumerator = resourceSet.GetEnumerator();
            while (enumerator.MoveNext())
            {
                resourceObject.Add(enumerator.Key.ToString(), enumerator.Value.ToString());
            }

            return Ok(resourceObject);
        }
    }
}
