var ClsItems = {

    GetAll: function () {

        Helper.AjaxCallGet(
            "https://localhost:7086/api/Items",
            {},
            "json",
            function (response) {

                var selectedBrands = [];

                document.querySelectorAll(".brand-filter:checked")
                    .forEach(function (checkbox) {
                        selectedBrands.push(checkbox.value);
                    });

                var maxPrice = document.getElementById("Price")?.value;

               
                var filteredItems = response.data.filter(function (item) {

                    var brandMatch =
                        selectedBrands.length === 0 ||
                        selectedBrands.includes(item.categoryName);

                    var priceMatch =
                        !maxPrice ||
                        item.salesPrice <= parseFloat(maxPrice);

                    return brandMatch && priceMatch;

                });

                
                $('#ItemPagination').pagination({

                    dataSource: filteredItems,

                    pageSize: 8,

                    showGoInput: true,

                    showGoButton: true,

                    callback: function (data, pagination) {

                        var htmlData = "";

                        for (var i = 0; i < data.length; i++) {

                            htmlData += ClsItems.DrawItem(data[i]);

                        }

                        $("#ItemArea").html(htmlData);

                    }

                });

            });

    },

DrawItem: function (item) {

    var data = "";

    data += "<div class='col-xl-3 col-6 col-grid-box'>";
    data += "<div class='product-box'>";
    data += "<div class='img-wrapper'>";

    data += "<div class='front'>";
    data += "<a href='/Items/ItemsDetails/" + item.itemId + "'>";
    data += "<img src='/Uploads/Items/" + item.imageName + "' class='img-fluid blur-up lazyload bg-img' alt=''>";
    data += "</a>";
    data += "</div>";

    data += "<div class='back'>";
    data += "<a href='/Items/ItemsDetails/" + item.itemId + "'>";
    data += "<img src='/Uploads/Items/" + item.imageName + "' class='img-fluid blur-up lazyload bg-img' alt=''>";
    data += "</a>";
    data += "</div>";

    data += "<div class='cart-info cart-wrap'>";

    data += "<a href='/Order/AddToCart?itemId=" + item.itemId + "' title='Add to Cart'>";
    data += "<i class='ti-shopping-cart'></i>";
    data += "</a>";

    data += "<a href='javascript:void(0)' title='Wishlist'>";
    data += "<i class='ti-heart'></i>";
    data += "</a>";

    data += "<a href='/Items/ItemsDetails/" + item.itemId + "' title='Quick View'>";
    data += "<i class='ti-search'></i>";
    data += "</a>";

    data += "<a href='#' title='Compare'>";
    data += "<i class='ti-reload'></i>";
    data += "</a>";

    data += "</div>";

    data += "</div>";

    data += "<div class='product-detail'>";

    data += "<a href='/Items/ItemsDetails/" + item.itemId + "'>";
    data += "<h6>" + item.itemName + "</h6>";
    data += "</a>";

    data += "<h4>$" + item.salesPrice + "</h4>";

    data += "</div>";
    data += "</div>";
    data += "</div>";

    return data;
}

};

document.addEventListener("change", function (e) {

    if (e.target.classList.contains("brand-filter")) {
        ClsItems.GetAll();
    }

    if (e.target.id === "Price") {
        ClsItems.GetAll();
    }

});