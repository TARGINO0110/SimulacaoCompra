using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimulacaoCompra.Models;

namespace SimulacaoCompra.Controllers
{
    public class ComprasController : Controller
    {
        private readonly BancoDbContext _context;

        public ComprasController(BancoDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            TempData["Message"] = "Olá, Seja bem vindo(a) ao simulador de compras, Hoje é dia: " + DateTime.Today.ToString("dd/MM/yyyy");
            return View(await _context.Compras.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Idcompra,Valortotal,Valorjuros,Qtdparcelas,Datacompra,Valorparcela,TipoCalculo")] Compra compra)
        {
           

            try
            {// calculo do juros normal
                if ((ModelState.IsValid) && (compra.Datacompra >= DateTime.Today) && (compra.TipoCalculo == "Normal"))
                {
                    _context.Add(compra);
                    compra.Valorparcela = compra.Valortotal * compra.Valorjuros + compra.Valortotal / compra.Qtdparcelas;
                    await _context.SaveChangesAsync();
                    TempData["Salvo"] = "A simulação foi salva com sucesso, o valor da sua parcela em " + compra.Qtdparcelas + "X é de R$ " + compra.Valorparcela.ToString("0.00");
                    return RedirectToAction(nameof(Index));
                }
                // calculo do juros Simples
                else if ((ModelState.IsValid) && (compra.Datacompra >= DateTime.Today) && (compra.TipoCalculo == "J.Simples"))
                {
                    _context.Add(compra);
                    compra.Valorparcela = compra.Valortotal * compra.Valorjuros * compra.Qtdparcelas / compra.Qtdparcelas;
                    await _context.SaveChangesAsync();
                    TempData["Salvo"] = "A simulação foi salva com sucesso, o valor da sua parcela em " + compra.Qtdparcelas + "X é de R$ " + compra.Valorparcela.ToString("0.00");
                    return RedirectToAction(nameof(Index));

                }
                // calculo do juros Composto
                else if ((ModelState.IsValid) && (compra.Datacompra >= DateTime.Today) && (compra.TipoCalculo == "J.Composto"))
                {
                    _context.Add(compra);
                    compra.Valorparcela = compra.Valortotal * (1 + compra.Valorjuros * compra.Qtdparcelas);
                    await _context.SaveChangesAsync();
                    TempData["Salvo"] = "A simulação foi salva com sucesso, o valor da sua parcela em " + compra.Qtdparcelas + "X é de R$ " + compra.Valorparcela.ToString("0.00");
                    return RedirectToAction(nameof(Index));

                }

   

            
                else
                {
                    TempData["ErroSalvar"] = "A data das compras devem ser para hoje ou posteriormente!";
                    return View("Create");
                }
                
            }
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
        public async Task<IActionResult> Edit(int id, [Bind("Idcompra,Valortotal,Valorjuros,Qtdparcelas,Datacompra,Valorparcela,TipoCalculo")] Compra compra)
        {
            try
            {
                if (id != compra.Idcompra)
                {
                    return NotFound();
                }

                if ((ModelState.IsValid) && (compra.Datacompra >= DateTime.Today) && (compra.TipoCalculo == "Normal"))
                {
                   
                    try
                    {
                        _context.Update(compra);
                        compra.Valorparcela = compra.Valortotal * compra.Valorjuros + compra.Valortotal / compra.Qtdparcelas;
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

                else if ((ModelState.IsValid) && (compra.Datacompra >= DateTime.Today) && (compra.TipoCalculo == "J.Simples"))
                {
                    
                    try
                    {
                        _context.Update(compra);
                        compra.Valorparcela = compra.Valortotal * compra.Valorjuros * compra.Qtdparcelas / compra.Qtdparcelas;
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
                        compra.Valorparcela = compra.Valortotal * (1 + compra.Valorjuros * compra.Qtdparcelas);
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
            catch(Exception)
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
                TempData["Deletar"] = "A simulação do valor da compra de R$ "+ compra.Valortotal + " com a parcela em " + compra.Qtdparcelas + "X de R$ "+ compra.Valorparcela + " na data "+ compra.Datacompra.ToString("dd/MM/yyyy")+" foi deletada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception)
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
