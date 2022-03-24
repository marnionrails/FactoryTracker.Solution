using System.Collections.Generic;
using System.Linq;

namespace Factory.Models
{
  public class Engineer
  {
    public Engineer()
    {
      this.JoinEngineerMachine = new HashSet<EngineerMachine>();
    }

    public int EngineerId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<EngineerMachine> JoinEngineerMachine { get; set; }

    public bool isDuplicateMachine(FactoryContext _db, int machineId)
    {
      var machines =  _db.EngineerMachines.Where(machine => machine.EngineerId == this.EngineerId).ToList();
      bool isDuplicate = false;
      foreach (var machine in machines)
      {
        if (machineId == machine.MachineId)
        {
          isDuplicate = true;
        }
      }
      return isDuplicate;
    }
  }
}