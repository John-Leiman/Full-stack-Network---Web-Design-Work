using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromoVisitor.Models
{
    public class PromoData
    {
        public string promoId { get; set; }
        public string bucketId { get; set; }
        public string brand { get; set; }
        public string slug { get; set; }
        public EnId titlePromo { get; set; }
        public EnId summaryPromo { get; set; }
        public EnId deskripsi { get; set; }
        public FourRecs rec { get; set; }
        public MetaTDK meta { get; set; }
        public string[] product { get; set; }
        public string[] lob { get; set; }
        public string[] area { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string orderDate { get; set; }
        public string status { get; set; }
        public string linkPage { get; set; }
        public int hideSettings { get; set; }
        public string changeFrequency { get; set; }
        public string priority { get; set; }
        public string createdBy { get; set; }
        public string createdByName { get; set; }
        public string createdDate { get; set; }
        public string publishDate { get; set; }
        public string updatedBy { get; set; }
        public string updatedByName { get; set; }
        public string updatedDate { get; set; }
        public string lastApprovedBy { get; set; }
        public string lastApprovedByName { get; set; }
        public string lastApprovedDate { get; set; }
        public string namaFileCover { get; set; }
        public string namaFileThumbnail { get; set; }
        public string fileCover { get; set; }
        public string fileThumbnail { get; set; }
        public string lockedBy { get; set; }
        public string lockedByName { get; set; }
        public InlineScripts inline { get; set; }
        public int activeVersion { get; set; }
        public int isVersioning { get; set; }
    }

    public class EnId
    {
        public string en;
        public string id { get; set; }
    }

    public class FourRecs
    {
        public string rec1 { get; set; }
        public string rec2 { get; set; }
        public string rec3 { get; set; }
        public string rec4 { get; set; }
    }

    public class MetaTDK
    {
        public string metaTitle;
        public string metaDescription;
        public string metaKeyword;
    }

    public class InlineScripts
    {
        public string inlineCss;
        public string inlineJs;
    }

    public class Category
    {
        public string CategoryID { get; set; }
        public string Title { get; set; }
        public string CategoryType { get; set; }
    }

    public class CleanData
    {
        public CleanData(PromoData promoData, string products, string lobs, string areas)
        {
            promoId = promoData.promoId;
            bucketId = promoData.bucketId;
            brand = promoData.brand;
            slug = promoData.slug;
            titlePromo = promoData.titlePromo;
            summaryPromo = promoData.summaryPromo;
            deskripsi = promoData.deskripsi;
            rec = promoData.rec;
            meta = promoData.meta;
            startDate = promoData.startDate;
            endDate = promoData.endDate;
            orderDate = promoData.orderDate;
            status = promoData.status;
            linkPage = promoData.linkPage;
            hideSettings = promoData.hideSettings;
            changeFrequency = promoData.changeFrequency;
            priority = promoData.priority;
            createdBy = promoData.createdBy;
            createdByName = promoData.createdByName;
            createdDate = promoData.createdDate;
            publishDate = promoData.publishDate;
            updatedBy = promoData.updatedBy;
            updatedByName = promoData.updatedByName;
            updatedDate = promoData.updatedDate;
            lastApprovedBy = promoData.lastApprovedBy;
            lastApprovedByName = promoData.lastApprovedByName;
            lastApprovedDate = promoData.lastApprovedDate;
            namaFileCover = promoData.namaFileCover;
            namaFileThumbnail = promoData.namaFileThumbnail;
            fileCover = promoData.fileCover;
            fileThumbnail = promoData.fileThumbnail;
            lockedBy = promoData.lockedBy;
            lockedByName = promoData.lockedByName;
            inline = promoData.inline;
            activeVersion = promoData.activeVersion;
            isVersioning = promoData.isVersioning;
            product = products;
            lob = lobs;
            area = areas;
        }
        public string promoId { get; set; }
        public string bucketId { get; set; }
        public string brand { get; set; }
        public string slug { get; set; }
        public EnId titlePromo { get; set; }
        public EnId summaryPromo { get; set; }
        public EnId deskripsi { get; set; }
        public FourRecs rec { get; set; }
        public MetaTDK meta { get; set; }
        public string product { get; set; }
        public string lob { get; set; }
        public string area { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string orderDate { get; set; }
        public string status { get; set; }
        public string linkPage { get; set; }
        public int hideSettings { get; set; }
        public string changeFrequency { get; set; }
        public string priority { get; set; }
        public string createdBy { get; set; }
        public string createdByName { get; set; }
        public string createdDate { get; set; }
        public string publishDate { get; set; }
        public string updatedBy { get; set; }
        public string updatedByName { get; set; }
        public string updatedDate { get; set; }
        public string lastApprovedBy { get; set; }
        public string lastApprovedByName { get; set; }
        public string lastApprovedDate { get; set; }
        public string namaFileCover { get; set; }
        public string namaFileThumbnail { get; set; }
        public string fileCover { get; set; }
        public string fileThumbnail { get; set; }
        public string lockedBy { get; set; }
        public string lockedByName { get; set; }
        public InlineScripts inline { get; set; }
        public int activeVersion { get; set; }
        public int isVersioning { get; set; }
    }

    public class PaginatedResult
    {
        public PaginatedResult(List<CleanData> cleanList, int PageTotal, int ResultTotal, int CurrentPage, int CurrentResult, int itemLoad, String order)
        {
            pageInfo = new PageInfo(PageTotal, ResultTotal, CurrentPage, CurrentResult, itemLoad, order);
            listPromo = cleanList;
        }
        public PageInfo pageInfo { get; set; }
        public List<CleanData> listPromo { get; set; }
    }

    public class PageInfo
    {
        public PageInfo(int pageTotal, int resultTotal, int currentPage, int currentResult, int itemLoad, String order)
        {
            PageTotal = pageTotal;
            ResultTotal = resultTotal;
            CurrentPage = currentPage;
            CurrentResult = currentResult;
            ItemLoad = itemLoad;
            Order = order;
        }

        public int PageTotal { get; set; }
        public int ResultTotal { get; set; }
        public int CurrentPage { get; set; }
        public int CurrentResult { get; set; }
        public int ItemLoad { get; set; }
        public String Order { get; set; }
    }

    public class ElasticResponse
    {
        public int took;
        public Boolean timed_out;
        public ElasticResponseShards _shards;
        public ElasticDataSet hits;
    }

    public class ElasticResponseShards
    {
        public int total;
        public int successful;
        public int skipped;
        public int failed;
    }

    public class ElasticDataSet
    {
        public ElasticDataSetTotal total;
        public int max_score;
        public List<ElasticData> hits { get; set; }
    }

    public class ElasticDataSetTotal
    {
        public int value;
        public String hrelationits;
    }

    public class ElasticData
    {
        public string brand;
        public string titlePromoEN;
        public string titlePromoID;
        public string summaryPromoEN;
        public string summaryPromoID;
        public string deskripsiEN;
        public string deskripsiID;
        public string metaKeyword;
        public string[] product;
        public string[] lob;
        public string[] area;
        public string startDate;
        public string endDate;
        public string orderDate;
        public string publishDate;
        public string linkPage;
        public int hideSettings;
        public string bucketId;
    }

    public class Parameters
    {
        public string categoryType;
        public string category;
        public string order;
        public int page;
        public int itemLoad;
        public string searchId;
    }

    public class ElasticParameters
    {
        public ElasticParameters(string bucketId, string sortBy, string ascDesc, int from, int size, string searchText, string products, string lobs, string areas)
        {
            this.bucketId = bucketId;
            this.sortBy = sortBy;
            this.ascDesc = ascDesc;
            this.from = from;
            this.size = size;       
            this.searchText = searchText;
            this.products = products;
            this.lobs = lobs;
            this.areas = areas;
        }

        public string bucketId;
        public string sortBy;
        public string ascDesc;
        public int from;
        public int size;
        public string searchText;
        public string products;
        public string lobs;
        public string areas;
    }
}