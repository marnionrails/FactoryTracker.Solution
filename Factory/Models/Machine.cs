using System;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Models
{
  public class Machine
  {
    public Machine()
    {
      this.JoinEngineerMachine = new HashSet<EngineerMachine>();
    }

    public int MachineId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<EngineerMachine> JoinEngineerMachine { get; set; }

    public bool isDuplicateDoctor(FactoryContext _db, int engineerId)
    {
      var engineers =  _db.EngineerMachines.Where(engineer => engineer.MachineId == this.MachineId).ToList();
      bool isDuplicate = false;
      foreach (var engineer in engineers)
      {
        if (engineerId == engineer.EngineerId)
        {
          isDuplicate = true;
        }
      }
      return isDuplicate;
    }
  }
}