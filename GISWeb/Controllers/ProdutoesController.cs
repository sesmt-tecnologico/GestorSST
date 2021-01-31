
using System;
using System.Collections.Generic;
using System.Data;
using GISCore.Infrastructure.Utils;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Business.Abstract;
using GISCore.Repository.Configuration;
using GISModel.Entidades.Estoques;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using GISWeb.Infraestrutura.Provider.Abstract;

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ProdutoesController : Controller
    {

        #region
        [Inject]
        public IBaseBusiness<Produto> ProdutoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Categoria> CategoriaBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }
        #endregion


        private SESTECContext db = new SESTECContext();

        // GET: Produtoes
        public ActionResult Index()
        {
            return View(db.Produto.ToList());
        }

        // GET: Produtoes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produtoes/Create
        public ActionResult Create()
        {
            var categoria = CategoriaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            ViewBag.categoria = categoria;

            return View();
        }

        // POST: Produto/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Produto produto)
        {
            if (ModelState.IsValid)
            {

                produto.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                ProdutoBusiness.Inserir(produto);

                // Extensions.GravaCookie("MensagemSucesso", "O Produto '" + produto.Nome + "' foi cadastrado com sucesso!", 10);


            }

            return Json(new { Resultado = produto.UKCategoria }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListarProduto(string UKCategoria)
        {

            Guid UKcat = Guid.Parse(UKCategoria);

            var oProduto = ProdutoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKCategoria.Equals(UKcat)).ToList();

            ViewBag.Ukcategoria = UKcat;

            return PartialView("_ListarProduto", oProduto);

        }



        // GET: Produtoes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtoes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Produto produto)
        {
            if (ModelState.IsValid)
            {
                var oProdutos = ProdutoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(produto.UniqueKey));

                
                var total = produto.Qunatidade + oProdutos.Qunatidade; 

                oProdutos.Nome = produto.Nome;
                oProdutos.Qunatidade = total;
                oProdutos.PrecoUnit = produto.PrecoUnit;
                oProdutos.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                ProdutoBusiness.Alterar(oProdutos);

                return RedirectToAction("Index");
            }
            return View(produto);
        }

        // GET: Produtoes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Produto produto = db.Produto.Find(id);
            db.Produto.Remove(produto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
