using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class Symbol
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }


    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    
    public Symbol(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("name cannot be empty.");

        Name = name;
        CreatedAt = DateTime.Now;
    }

    public void Update()
    {
        UpdatedAt = DateTime.Now;
    }

    
    protected Symbol()
    { }
}

