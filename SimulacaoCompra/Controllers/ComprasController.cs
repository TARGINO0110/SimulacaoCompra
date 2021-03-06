﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulacaoCompra.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace SimulacaoCompra.Controllers
{
    public class ComprasController : Controller
    {
        private readonly BancoDbContext _context;

        public ComprasController(BancoDbContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Compra> Compras { get; set; }

        // GET: Compras inicializa a pagina inicial de compras retornando de modo atualizado a data atual.
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {

            TempData["Message"] = "Olá, Seja bem vindo(a) ao simulador de compras, Hoje é dia: " + DateTime.Today.ToString("dd/MM/yyyy");

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var compras = from c in _context.Compras
                          select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                compras = compras.Where(c => c.NomeDaCompra.Contains(searchString)
                                       || c.TipoCalculo.Contains(searchString)
                                       || c.Categoria.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    compras = compras.OrderByDescending(c => c.NomeDaCompra);
                    break;
                case "Date":
                    compras = compras.OrderBy(c => c.Datacompra);
                    break;
                case "date_desc":
                    compras = compras.OrderByDescending(s => s.Datacompra);
                    break;
                default:
                    compras = compras.OrderBy(s => s.Categoria);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Compra>.CreateAsync(
                compras.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .SingleOrDefaultAsync(m => m.Idcompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idcompra,NomeDaCompra,Valortotal,Valorjuros,Qtdparcelas,Datacompra,Valorparcela,TipoCalculo,Categoria")] Compra compra)
        {

            // Inicia-se aqui a condição do tipo do calculo escolhido pelo usuário sendo de juros simples ou composto.
            try
            { // calculo do juros Simples
                if ((ModelState.IsValid) && (compra.Datacompra >= DateTime.Today) && (compra.TipoCalculo == "J.Simples"))
                {
                    _context.Add(compra);
                    compra.Valorparcela = compra.Valortotal * compra.Valorjuros + compra.Valortotal;
                    compra.Valorparcela /= compra.Qtdparcelas;
                    await _context.SaveChangesAsync();
                    TempData["Salvo"] = "A simulação foi salva com sucesso, o valor da sua parcela em " + compra.Qtdparcelas + "X é de R$ " + compra.Valorparcela.ToString("0.00");
                    return RedirectToAction(nameof(Index));
                }


                // calculo do juros Composto
                else if ((ModelState.IsValid) && (compra.Datacompra >= DateTime.Today) && (compra.TipoCalculo == "J.Composto"))
                {

                    _context.Add(compra);
                    double M; //montante
                    double capital = Convert.ToDouble(compra.Valortotal); //capital
                    double taxa = Convert.ToDouble(compra.Valorjuros);//taxa juros
                    double Qtd = Convert.ToDouble(compra.Qtdparcelas);//parcela


                    M = capital * Math.Pow((1 + taxa), Qtd);//Formula Juros Composto


                    compra.Valorparcela = Convert.ToDecimal(M);
                    compra.Valorparcela /= compra.Qtdparcelas;

                    await _context.SaveChangesAsync();
                    TempData["Salvo"] = "A simulação foi salva com sucesso, o valor da sua parcela em " + compra.Qtdparcelas + "X é de R$ " + compra.Valorparcela.ToString("0.00");
                    return RedirectToAction(nameof(Index));

                }


                else
                {
                    TempData["ErroSalvar"] = "A data da compra ou serviço deve ser para data atual ou posterior!";
                    return View("Create");
                }

            } //Tratamento de exceção
            catch (Exception)
            {
                return View("Error");
            }
            finally
            {

            }

        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.SingleOrDefaultAsync(m => m.Idcompra == id);
            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idcompra,NomeDaCompra,Valortotal,Valorjuros,Qtdparcelas,Datacompra,Valorparcela,TipoCalculo,Categoria")] Compra compra)
        {
            try
            { // verifica a Id de cada cadastro salvo retornando caso seja existente na base de dados
                if (id != compra.Idcompra)
                {
                    return NotFound();
                }

                if ((ModelState.IsValid) && (compra.Datacompra >= DateTime.Today) && (compra.TipoCalculo == "J.Simples"))
                {

                    try
                    {
                        _context.Update(compra);
                        compra.Valorparcela = compra.Valortotal * compra.Valorjuros + compra.Valortotal;//Formula Juros Simples
                        compra.Valorparcela /= compra.Qtdparcelas;
                        await _context.SaveChangesAsync();
                        TempData["Salvo"] = "A simulação foi salva com sucesso, o valor da sua parcela em " + compra.Qtdparcelas + "X é de R$ " + compra.Valorparcela.ToString("0.00");

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CompraExists(compra.Idcompra))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }


                else if ((ModelState.IsValid) && (compra.Datacompra >= DateTime.Today) && (compra.TipoCalculo == "J.Composto"))
                {

                    try
                    {
                        _context.Update(compra);
                        double M; //montante
                        double capital = Convert.ToDouble(compra.Valortotal); //capital
                        double taxa = Convert.ToDouble(compra.Valorjuros);//taxa juros
                        double Qtd = Convert.ToDouble(compra.Qtdparcelas);//parcela


                        M = capital * Math.Pow((1 + taxa), Qtd);//Formula Juros Composto


                        compra.Valorparcela = Convert.ToDecimal(M);
                        compra.Valorparcela /= compra.Qtdparcelas;
                        await _context.SaveChangesAsync();
                        TempData["Salvo"] = "A simulação foi salva com sucesso, o valor da sua parcela em " + compra.Qtdparcelas + "X é de R$ " + compra.Valorparcela.ToString("0.00");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CompraExists(compra.Idcompra))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErroSalvar"] = "A data das compras devem ser para hoje ou posteriormente!";
                    return View("Edit");
                }
            }
            catch (Exception)
            {
                return View("Erro");
            }
            finally
            {

            }

        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .SingleOrDefaultAsync(m => m.Idcompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {

                var compra = await _context.Compras.SingleOrDefaultAsync(m => m.Idcompra == id);
                _context.Compras.Remove(compra);
                await _context.SaveChangesAsync();
                TempData["Deletar"] = "A simulação do valor da compra de R$ " + compra.Valortotal + " com a parcela em " + compra.Qtdparcelas.ToString("0") + "X de R$ " + compra.Valorparcela.ToString("0.00") + " na data " + compra.Datacompra.ToString("dd/MM/yyyy") + " foi deletada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("Index");
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.Idcompra == id);
        }


    }
}
