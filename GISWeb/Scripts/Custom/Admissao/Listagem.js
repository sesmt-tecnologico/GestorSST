

function BuscarDetalhesEstabelecimentoImagens(IDEstabelecimentoImagens) {
    
    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/AtividadesDoEstabelecimento/BuscarDetalhesEstabelecimentoImagens",
        data: { idEstabelecimentoImagens: IDEstabelecimentoImagens },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null)
            {
                bootbox.dialog
                   ({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes da Empresa2</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                  });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

}


//
function ListaExposicao(idAlocacao, idAtividadeAlocada,Nome, cpf, idAtividadeEstabelecimento) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Admissao/ListaExposicao",
        data: { idAlocacao: idAlocacao, idAtividadeAlocada: idAtividadeAlocada, Nome: Nome, cpf: cpf, idAtividadeEstabelecimento: idAtividadeEstabelecimento },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Exposição ao Risco</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

}


function ListaExpoAtivFuncao(idAlocacao, idAtividadeFuncaoLiberada, Nome, cpf, idAtividade) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Admissao/ListaExposicao",
        data: {
            idAlocacao: idAlocacao,
            idAtividadeFuncaoLiberada: idAtividadeFuncaoLiberada,
            Nome: Nome, cpf: cpf,
            idAtividade: idAtividade
        },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Exposição ao Risco</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

}





//
function ListarPlanoDeAcao(idTipoDeRisco) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/PlanoDeAcao/ListarPlanoDeAcao",
        data: { idTipoDeRisco: idTipoDeRisco },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Lista Plano de Ação</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

}

function BuscarDetalhesDeMedidasDeControleAtividadeFuncao(idTipoRisco, idAtividade) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Atividade/BuscarDetalhesDeMedidasDeControleAtividadeFuncao",
        data: {
            idTipoRisco: idTipoRisco,
            idAtividade: idAtividade
        },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Controles de Riscos</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });
}


function BuscarDetalhesDeMedidasDeControleEstabelecimento(id) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/AtividadesDoEstabelecimento/BuscarDetalhesDeMedidasDeControle",
        data: { id: id },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Controles de Riscos</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });
}



function DeletarEmpresa(IDAdmissao, Nome) {
    
    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Admissao/TerminarComRedirect",
            data: { IDAdmissao: IDAdmissao },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $("#linha-" + IDAdmissao).remove();
                }
            }
        });
    };



    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir esta admissao?", "Exclusão de Admissao", callback, "btn-danger");

    //ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir esta admissao '" + Nome + "'?", "Exclusão de Empresa", callback, "btn-danger");

}





function Exposicao(IDAtividadeAlocada, idAlocacao, idTipoDeRisco, idEmpregado) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Exposicao/Novo",
        data: { IDAtividadeAlocada: IDAtividadeAlocada, idAlocacao: idAlocacao, idTipoDeRisco: idTipoDeRisco, idEmpregado: idEmpregado},
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Cadastrar Exposicao do Empregado</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

};


function AlocarEmpregado(IDAdmissao, IDEmpregado, IDEmpresa) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Alocacao/Novo",
        data: { IDAdmissao: IDAdmissao, IDEmpregado: IDEmpregado, IDEmpresa: IDEmpresa },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Alocar Empregado</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

};


//jQuery(function ($) {

//    AplicajQdataTable("dynamic-table", [{ "bSortable": false },  null, { "bSortable": false }], false, 20);

//});
function AlocarEmAmbiente(idEstabelecimento, idAlocacao) {

    
    $(".LoadingLayout").show();
    //$('tablePerfisPorMenu').css({ opacity: "0.5" });
    $.ajax({
        method: "POST",
        url: "/AtividadesDoEstabelecimento/AlocarEmAmbiente",
        data: { idEstabelecimento: idEstabelecimento, idAlocacao: idAlocacao },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {          

                
            $(".LoadingLayout").hide();
            //$("tablePerfisPorMenu").css({ opacity: '' });
            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Selecione os Ambientes para este empregado</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else
            {
                TratarResultadoJSON(content.resultado);
            }

             AplicajQdataTable("tablePerfisPorMenu", [{ "bSortable": false }, null], false, 25);
            
        } 
            
          
    });

};


function AlocarAtivFuncao(IDFuncao, IDEmpregado, IDAlocacao) {


    $(".LoadingLayout").show();
    //$('tablePerfisPorMenu').css({ opacity: "0.5" });
    $.ajax({
        method: "POST",
        url: "/Atividade/AlocarAtivFuncao",
        data: {
            IDFuncao: IDFuncao,
            IDEmpregado: IDEmpregado,
            IDAlocacao: IDAlocacao
           
        },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {


            $(".LoadingLayout").hide();
            //$("tablePerfisPorMenu").css({ opacity: '' });
            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Selecione as Atividades para esta Função</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

            AplicajQdataTable("tablePerfisPorMenu", [{ "bSortable": false }, null], false, 25);

        }


    });

};



function PesquisarAtividadesRiscos(idEstabelecimento, idAlocacao) {


    $(".LoadingLayout").show();
    //$('tablePerfisPorMenu').css({ opacity: "0.5" });
    $.ajax({
        method: "POST",
        url: "/AnaliseRisco/PesquisarAtividadesRiscos",
        data: { idEstabelecimento: idEstabelecimento, idAlocacao: idAlocacao },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {


            $(".LoadingLayout").hide();
            //$("tablePerfisPorMenu").css({ opacity: '' });
            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Analise os Riscos desta Atividades</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

            AplicajQdataTable("tablePerfisPorMenu", [{ "bSortable": false }, null], false, 25);

        }


    });

};







function EstabelecimentoAmbienteAlocado(idEstabelecimento, idAlocacao, idAtividadeAlocada, idAtividadesDoEstabelecimento, idEmpregado) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/AtividadesDoEstabelecimento/EstabelecimentoAmbienteAlocado",
        data: { idEstabelecimento: idEstabelecimento, idAlocacao: idAlocacao, idAtividadeAlocada: idAtividadeAlocada, idAtividadesDoEstabelecimento: idAtividadesDoEstabelecimento, idEmpregado: idEmpregado },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Ambientes deste Empregado</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {

               

                TratarResultadoJSON(content.resultado);
            }

            AplicajQdataTable("RiscosRelacionadoAmbiente", [{ "bSortable": false },null, null], false, 25);


        }
    });

};

function AtividadesDaFuncao(idAtividade, idAlocacao, idAtivFuncaoLiberada, idEmpregado) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Atividade/AtividadesDaFuncao",
        data: {
            idAtividade: idAtividade,
            idAlocacao: idAlocacao,            
            idAtivFuncaoLiberada: idAtivFuncaoLiberada,
            idEmpregado: idEmpregado
        },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Atividades desta Função</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {



                TratarResultadoJSON(content.resultado);
            }

            AplicajQdataTable("RiscosRelacionadoAmbiente", [{ "bSortable": false }, null, null], false, 25);


        }
    });

};

function Ambiente(idEstabelecimento) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/AtividadesDoEstabelecimento/Ambiente",
        data: { idEstabelecimento: idEstabelecimento },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Atividades deste Empregado</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {



                TratarResultadoJSON(content.resultado);
            }

            AplicajQdataTable("RiscosRelacionadoAmbiente", [{ "bSortable": false }, null, null, null, null], false, 25);


        }
    });

};

           



    function DesalocarEmpregado(IDAlocacao,IDEmpregado) {

        var callback = function () {
            $('.LoadingLayout').show();
            $('#dynamic-table').css({ opacity: "0.5" });

            $.ajax({
                method: "POST",
                url: "/Alocacao/TerminarComRedirect",
                data: { IDAlocacao: IDAlocacao, IDEmpregado: IDEmpregado },
                error: function (erro) {
                    $(".LoadingLayout").hide();
                    $("#dynamic-table").css({ opacity: '' });
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                },
                success: function (content) {
                    $('.LoadingLayout').hide();
                    $("#dynamic-table").css({ opacity: '' });

                    TratarResultadoJSON(content.resultado);

                    if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                        $("#linha-" + IDAlocacao).remove();
                    }
                }
            });
        };

        ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja desalocar este empregado?", "Desalocar Empregado", callback, "btn-danger");
    };


    function OnSuccessCadastrarExposicao(data) {
        $('#formCadastroExposicao').removeAttr('style');
        $(".LoadingLayout").hide();
        $('#btnSalvar').show();
        TratarResultadoJSON(data.resultado);
        //ExibirMsgGritter(data.resultado);
        $('#dtExpo').disableSelection();
        $('#dtExpo01').disableSelection();
    }

    function OnBeginCadastrarExposicao() {
        $(".LoadingLayout").show();
        $('#blnSalvar').hide();
        $("#formCadastroExposicao").css({ opacity: "0.5" });

    }