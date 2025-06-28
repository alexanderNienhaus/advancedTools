using Unity.Collections;
using Unity.Entities;

public class CustomEvent
{
}

public class OnNumberOfParticlesChangedDOTS : CustomEvent
{
    public int numberOfParticlesChange;

    public OnNumberOfParticlesChangedDOTS(int pNumberOfParticlesChange)
    {
        numberOfParticlesChange = pNumberOfParticlesChange;
    }
}

public class OnNumberOfParticlesChangedStandard : CustomEvent
{
    public int numberOfParticlesChange;


    public OnNumberOfParticlesChangedStandard(int pNumberOfParticlesChange)
    {
        numberOfParticlesChange = pNumberOfParticlesChange;
    }
}