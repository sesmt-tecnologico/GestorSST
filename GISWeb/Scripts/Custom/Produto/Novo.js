function SalvarProduto() {


    var produto = $("#txtProduto").val();
    var UKcategoria = $("#txtCategoria").val();
    var Quant = $("#txtQuantidade").val();
    var preco = $("#txtPrecoUnit").val();
    var status = $("#txtstatus").val();
    var quantMininma = $("#txtQuantMinima").val();


    var token = $('input[name="__RequestVerificationToken"]').val();
    var tokenadr = $('form[action="/Produto/Create"] input[name="__RequestVerificationToken"]').val();

    var heardes = {};
    var headersadr = {};
    heardes['__RequestVerificationToken'] = token;
    headersadr['__RequestVerificationToken'] = tokenadr;

    var url = "/Produtoes/Create";

    $.ajax({

        url: url,
        type: "POST",
        datatype:"json",
        heardes: headersadr,
        data: {
            Nome: produto, UKCategoria: UKcategoria, Qunatidade: Quant, PrecoUnit: preco,
            status: status, QunatMinima: quantMininma, __RequestVerificationToken: token
        },
        success: function (data) {
            if (data.Resultado != null) {
                ListarProduto(data.Resultado);                

                alert( produto + " - SALVO COM SUCESSO!");

                produto = $("#txtProduto").val("");
                Quant = $("#txtQuantidade").val("");
                preco = $("#txtPrecoUnit").val("");
            }
        }

    });
}

function ListarProduto(ukcategoria) {

    var url = "/Produtoes/ListarProduto";

    $.ajax({

        url: url,
        type: "GET",
        data: { UKCategoria: ukcategoria },
        datatype: "html",
        success: function (data) {
            var divProdutos = $("#divProdutos");
            divProdutos.empty();
            divProdutos.show();
            divProdutos.html(data);
        }



    });

}