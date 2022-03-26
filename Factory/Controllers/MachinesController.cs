using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class MachinesController : Controller
  {
    private readonly FactoryContext _db;

    public MachinesController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Machines.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Machine Machine, int EngineerId)
    {
      _db.Machines.Add(Machine);
      _db.SaveChanges();
      if (EngineerId != 0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() {EngineerId = EngineerId, MachineId = Machine.MachineId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Machine foundMachine = _db.Machines
        .Include(Machine => Machine.JoinEngineerMachine)
        .ThenInclude(joinEngineer => joinEngineer.Engineer)
        .FirstOrDefault(model => model.MachineId == id);
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
      return View(foundMachine);
    }

    public ActionResult Edit(int id)
    {
      var foundMachine = _db.Machines.FirstOrDefault(Machine => Machine.MachineId == id);
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
      return View(foundMachine);
    }

    [HttpPost]
    public ActionResult Edit(Machine Machine, int EngineerId)
    {
      if (EngineerId !=0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() {EngineerId = EngineerId, MachineId = Machine.MachineId});        
      }
      _db.Entry(Machine).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Machine foundMachine = _db.Machines.FirstOrDefault(Machine => Machine.MachineId == id);
      return View(foundMachine);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Machine foundMachine = _db.Machines.FirstOrDefault(Machine => Machine.MachineId == id);
      _db.Machines.Remove(foundMachine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult AddEngineer(Machine Machine, int EngineerId)
    {
      bool isDuplicate = Machine.isDuplicateEngineer(_db, EngineerId);
      if (EngineerId !=0 && isDuplicate == false)
      {
        _db.EngineerMachines.Add(new EngineerMachine() {EngineerId = EngineerId, MachineId = Machine.MachineId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = Machine.MachineId});
    }

    [HttpPost]
    public ActionResult DeleteEngineer(int joinId)
    {
      var joinEntry = _db.EngineerMachines.FirstOrDefault(entry => entry.EngineerMachineId == joinId);
      int savedMachine = joinEntry.MachineId;
      _db.EngineerMachines.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new {id = savedMachine});
    }
  }
}