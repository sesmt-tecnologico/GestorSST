using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Repository.Configuration;
using GISModel.Entidades.ArquivoFiscal;
using GISWeb.Infraestrutura.Filters;

namespace GISWeb.Controllers.ArquivoFiscal
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NotaFiscalsController : Controller
    {
        private SESTECContext db = new SESTECContext();

        // GET: NotaFiscals
        public ActionResult Index()
        {
            return View(db.NotaFiscal.ToList());
        }

        // GET: NotaFiscals/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotaFiscal notaFiscal = db.NotaFiscal.Find(id);
            if (notaFiscal == null)
            {
                return HttpNotFound();
            }
            return View(notaFiscal);
        }

        // GET: NotaFiscals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NotaFiscals/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Servico,Material,ServicoMaterial,Entrada,Saida,Vencimento,Descricao,Fornecedor,Numero,Valor,UniqueKey,UsuarioInclusao,DataInclusao,UsuarioExclusao,DataExclusao")] NotaFiscal notaFiscal)
        {
            if (ModelState.IsValid)
            {
                notaFiscal.ID = Guid.NewGuid();
                db.NotaFiscal.Add(notaFiscal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(notaFiscal);
        }

        // GET: NotaFiscals/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotaFiscal notaFiscal = db.NotaFiscal.Find(id);
            if (notaFiscal == null)
            {
                return HttpNotFound();
            }
            return View(notaFiscal);
        }

        // POST: NotaFiscals/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Servico,Material,ServicoMaterial,Entrada,Saida,Vencimento,Descricao,Fornecedor,Numero,Valor,UniqueKey,UsuarioInclusao,DataInclusao,UsuarioExclusao,DataExclusao")] NotaFiscal notaFiscal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notaFiscal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notaFiscal);
        }

        // GET: NotaFiscals/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotaFiscal notaFiscal = db.NotaFiscal.Find(id);
            if (notaFiscal == null)
            {
                return HttpNotFound();
            }
            return View(notaFiscal);
        }

        // POST: NotaFiscals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            NotaFiscal notaFiscal = db.NotaFiscal.Find(id);
            db.NotaFiscal.Remove(notaFiscal);
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
