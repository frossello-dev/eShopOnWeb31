﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.eShopWeb.Web.Extensions;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Pages.Admin
{
    [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
    public class ProductsModel : PageModel
    {
        private readonly ICatalogViewModelService _catalogViewModelService;
        private readonly IMemoryCache _cache;

        public ProductsModel(ICatalogViewModelService catalogViewModelService, IMemoryCache cache)
        {
            _catalogViewModelService = catalogViewModelService;
            _cache = cache;
        }

        public CatalogProductsViewModel CatalogModel { get; set; } = new CatalogProductsViewModel();

        public async Task OnGet(CatalogProductsViewModel catalogModel, int? pageId)
        {
            var cacheKey = CacheHelpers.GenerateCatalogItemCacheKey(pageId.GetValueOrDefault(), Constants.ITEMS_PER_PAGE, catalogModel.BrandFilterApplied, catalogModel.TypesFilterApplied);

            _cache.Remove(cacheKey);

            CatalogModel = await _catalogViewModelService.GetCatalogProducts(pageId.GetValueOrDefault(), Constants.ITEMS_PER_PAGE, catalogModel.BrandFilterApplied, catalogModel.TypesFilterApplied).ConfigureAwait(true);
        }
    }
}
