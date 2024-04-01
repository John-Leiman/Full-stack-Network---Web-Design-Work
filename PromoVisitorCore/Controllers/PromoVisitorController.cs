using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PromoVisitor.Models;
using PromoVisitor.Repositories;
using System.IO;
using System.Net;
using System.Xml;
using System.Diagnostics;

namespace PromoVisitor.Controllers
{
    public class PromoVisitorController : Controller
    {
        public static PromoVisitorRepository promoVisitorRepository;
        private List<Category> categoryList;
        private String method;
        private Parameters callPars;
        private ElasticParameters elaPars;

        public PromoVisitorController()
        {
            promoVisitorRepository = new PromoVisitorRepository();
            categoryList = promoVisitorRepository.categorize();
            method = string.Empty;
        }

        // GET: PromoVisitor/Index
        [HttpGet]
        public ActionResult Index(string categoryType = null, string category = null, string order = "ASCENDING", int page = 1, int itemLoad = 4)
        {
            try
            {
                Response.AppendHeader("Access-Control-Allow-Origin", "*");
                
                method = "Index";
                callPars = new Parameters();
                callPars.categoryType = categoryType;
                callPars.category = category;
                callPars.order = order;
                callPars.page = page;
                callPars.itemLoad = itemLoad;

                if (itemLoad < 1 || 12 < itemLoad)
                {
                    return respond(400, "FAILED: itemLoad must be between 1 and 12!");
                }

                if (page < 1)
                {
                    return respond(400, "FAILED: page must be greater than 1!");
                }

                String[] orders = { "ASCENDING", "DESCENDING", "DATE" };
                if (!orders.Contains(order))
                {
                    return respond(400, "FAILED: Invalid order!");
                }

                String[] categoryTypes = { "product", "lob", "area", null };
                if (!categoryTypes.Contains(categoryType))
                {
                    return respond(400, "FAILED: Invalid categoryType!");
                }

                if (string.IsNullOrEmpty(categoryType) ^ string.IsNullOrEmpty(category))
                {
                    return respond(400, "FAILED: Provide category AND categoryType to fitler results!");
                }

                if (categoryType != null)
                {
                    Boolean match = false;
                    foreach (Category cat in categoryList)
                    {
                        if (categoryType.ToUpper().Equals(cat.CategoryType) && category.Equals(cat.Title))
                        {
                            match = true;
                            break;
                        }
                    }
                    if (!match)
                    {
                        return respond(400, "FAILED: category/categoryType mismatch!");
                    }
                }

                List<PromoData> dataList = promoVisitorRepository.promoAll(order);
                if (dataList.Count == 0)
                {
                    return respond(404, "FAILED: Promos not found!", dataList);
                }

                List<CleanData> cleanList = new List<CleanData>();
                int first = (page - 1) * itemLoad;
                int pages = (dataList.Count + itemLoad - 1) / itemLoad;
                Debug.Print(dataList.Count.ToString());
                Debug.Print(pages.ToString());
                int last;
                int total;
                if (page > pages)
                {
                    return respond(400, "FAILED: Page index out-of-bounds!");
                }
                if (categoryType == null)
                {
                    last = page == pages || pages == 0 ? dataList.Count : page * itemLoad;
                    for (int i = first; i < last; i++)
                    {
                        cleanList.Add(clean(dataList[i]));
                    }
                    total = dataList.Count;
                }
                else
                {
                    foreach (PromoData promoData in dataList)
                    {
                        CleanData cleanData = clean(promoData);
                        if (typeof(CleanData).GetProperty(categoryType).GetValue(cleanData).ToString().Contains(category))
                        {
                            cleanList.Add(cleanData);
                        }
                    }
                    if (cleanList.Count == 0)
                    {
                        return respond(404, "FAILED: Promos in the category not found!", cleanList);
                    }
                    pages = (cleanList.Count + itemLoad - 1) / itemLoad;
                    last = page == pages || pages == 0 ? cleanList.Count : page * itemLoad;
                    total = cleanList.Count;
                    if (page > pages)
                    {
                        return respond(400, "FAILED: Page index out-of=bounds!");
                    }
                }

                return respond(200, "SUCCESS", new PaginatedResult(cleanList, pages, total, page, last - first, itemLoad, order));
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return respond(500, "FAILED: System Error!", ex.ToString());
            }
        }

        // GET: PromoVisitor/getLinkPromo
        [HttpGet]
        public ActionResult getLinkPromo(string searchId)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            method = "getLinkPromo";
            callPars = new Parameters();
            callPars.categoryType = searchId;

            if (string.IsNullOrWhiteSpace(searchId) || !(searchId[0] == '/') || searchId.Contains(" "))
            {
                return respond(400, "FAILED: Invalid searchId!");
            }

            try
            {
                PromoData promoData = promoVisitorRepository.promoSearch(searchId);
                if (promoData == null)
                {
                    return respond(404, "FAILED: Promo not found!");
                }

                return respond(200, "SUCCESS", clean(promoData));
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return respond(500, "FAILED: System Error!", ex.ToString());
            }
        }

        // GET: PromoVisitor/getRecPromos
        [HttpGet]
        public ActionResult getRecPromos(string searchId)
        {
            try
            {
                Response.AppendHeader("Access-Control-Allow-Origin", "*");

                method = "getRecPromos";
                callPars = new Parameters();
                callPars.categoryType = searchId;

                if (string.IsNullOrWhiteSpace(searchId) || !(searchId[0] == '/') || searchId.Contains(" "))
                {
                    return respond(400, "FAILED: Invalid searchId!");
                }

                PromoData promoDatum = promoVisitorRepository.promoSearch(searchId);
                if (promoDatum == null)
                {
                    return respond(404, "FAILED: Promo not found!");
                }

                List<PromoData> fourRecs = new List<PromoData>();
                fourRecs.Add(promoVisitorRepository.promoSearch(promoDatum.rec.rec1));
                fourRecs.Add(promoVisitorRepository.promoSearch(promoDatum.rec.rec2));
                fourRecs.Add(promoVisitorRepository.promoSearch(promoDatum.rec.rec3));
                fourRecs.Add(promoVisitorRepository.promoSearch(promoDatum.rec.rec4));
                List<CleanData> cleanRecs = new List<CleanData>();
                foreach (PromoData rec in fourRecs)
                {
                    if (rec != null)
                    {
                        cleanRecs.Add(clean(rec));
                    }
                }
                int recsLeft = 4 - cleanRecs.Count;
                if (recsLeft == 0)
                {
                    return respond(200, "SUCCESS", cleanRecs);
                }

                List<PromoData> dataList = promoVisitorRepository.promoAll();
                if (dataList.Count == 1)
                {
                    return respond(404, "FAILED: Recommended Promos not found!");
                }

                CleanData cleanDatum = clean(promoDatum);
                cleanRecs = fillRecs(true, cleanDatum, cleanRecs, recsLeft, dataList);
                recsLeft = 4 - cleanRecs.Count;
                if (recsLeft == 0)
                {
                    return respond(200, "SUCCESS", cleanRecs);
                }

                return respond(200, "SUCCESS", fillRecs(false, cleanDatum, cleanRecs, recsLeft, dataList));
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return respond(500, "FAILED: System Error!", ex.ToString());
            }
        }

        // GET: PromoVisitor/getElasticPromos
        [HttpGet]
        public ActionResult getElasticPromos(String bucketId, String sortBy = "_score", String ascDesc = "asc", int from = 1, int size = 4, String searchText = "", String products = "", String lobs = "", String areas = "")
        {
            try
            {
                method = "getElasticPromos";
                elaPars = new ElasticParameters(bucketId, sortBy, ascDesc, from, size, searchText, products, lobs, areas);
                if (string.IsNullOrWhiteSpace(bucketId))
                {
                    return respond(400, "FAILED: Invalid bucketId!");
                }
                var elasticResponse = promoVisitorRepository.searchElastic(bucketId, sortBy, ascDesc, from, size, searchText, products, lobs, areas);
                return respond(200, "SUCCESS", elasticResponse);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return respond(500, "FAILED: System Error!", ex.ToString());
            }
        }

        private List<CleanData> fillRecs(bool filtering, CleanData cleanDatum, List<CleanData> cleanRecs, int recsLeft, List<PromoData> dataList)
        {
            List<string> recIDList = new List<string>();
            recIDList.Add(cleanDatum.promoId);
            foreach (CleanData cleanRec in cleanRecs)
            {
                recIDList.Add(cleanRec.promoId);
            }
            String lobCategory = cleanDatum.lob;
            List<CleanData> cleanList = new List<CleanData>();
            if (filtering)
            {
                foreach (PromoData promoData in dataList)
                {
                    CleanData cleanData = clean(promoData);
                    if (cleanData.lob.Contains(lobCategory))
                    {
                        cleanList.Add(cleanData);
                    }
                }
            }
            else
            {
                foreach (PromoData promoData in dataList)
                {
                    CleanData cleanData = clean(promoData);
                    cleanList.Add(cleanData);
                }
            }
            foreach (CleanData cleanPromo in cleanList)
            {
                if (recsLeft == 0)
                {
                    break;
                }
                if (recIDList.Contains(cleanPromo.promoId))
                {
                    recsLeft++;
                }
                else
                {
                    cleanRecs.Add(cleanPromo);
                    recsLeft--;
                }
            }
            return cleanRecs;
        }

        private CleanData clean(PromoData promoData)
        {
            string products = "";
            string lobs = "";
            string areas = "";

            string[] product = promoData.product;
            string[] lob = promoData.lob;
            string[] area = promoData.area;

            foreach (Category category in categoryList)
            {
                string categoryID = category.CategoryID.ToUpper();
                if (product.Contains(categoryID))
                {
                    products += category.Title + ", ";
                }
                else if (lob.Contains(categoryID))
                {
                    lobs += category.Title + ", ";
                }
                else if (area.Contains(categoryID))
                {
                    areas += category.Title + ", ";
                }
            }
            char[] trimmer = { ',', ' ' };
            products = products.TrimEnd(trimmer);
            lobs = lobs.TrimEnd(trimmer);
            areas = areas.TrimEnd(trimmer);

            CleanData processedData = new CleanData(promoData, products, lobs, areas);
            return processedData;
        }

        private ActionResult respond(int status, String errMess, Object result = null)
        {
            promoVisitorRepository.log(status, errMess, method, callPars, elaPars);
            return result == null ? Json(new { Status = status, ErrorMessage = errMess }, JsonRequestBehavior.AllowGet) : Json(new { Status = status, ErrorMessage = errMess, Result = result }, JsonRequestBehavior.AllowGet);
        }
    }
}