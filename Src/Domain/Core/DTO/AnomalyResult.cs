using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO;

public record AnomalyResult(string Symbol, decimal StartPrice, decimal EndPrice, decimal ChangePercent);

