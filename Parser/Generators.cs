using System;
using System.Text;
using System.Collections.Generic;

namespace Generators
{

  // TODO: tree generation requires shared state - the size - this should be the meaning of SizedGenerator
  // TODO: generalize to Context generator, that takes any context? Size is built in.
  // TODO: we should pass random when generating, not before.

  public abstract class Generator<T>
  {
    public abstract T Generate(Random rnd);

    public IEnumerable<T> Generate(Random rnd, int count)
    {
      for (int i = 0; i < count; i++)
      {
        yield return Generate(rnd);
      }
    }

  }



  public abstract class SizedGenerator<T> : Generator<T>
  {
    protected int max;
    protected int min;

    public SizedGenerator(int min, int max)
    {
      this.min = min;
      this.max = max;
    }

    public SizedGenerator(int target, float ep)
    {
      double dtarget = (float)target;
      this.max = (int)Math.Ceiling(dtarget + dtarget * ep);
      this.min = (int)Math.Floor(dtarget - dtarget * ep);

      if (min < 0) throw new ArithmeticException();
      if (max < min) throw new ArithmeticException();
    }

    public int Within(Random rnd)
    {
      return rnd.Next(min, max);
    }

  }

  public class UniformSelector<T> : Generator<T>
  {
    readonly IList<T> elements;

    public UniformSelector(IList<T> elements)
    {
      this.elements = elements;
    }

    public override T Generate(Random rnd)
    {
      var ix = rnd.Next(elements.Count);
      return elements[ix];
    }

  }

  public class OneOf<T> : Generator<T>
  {
    readonly IList<Generator<T>> generators;
    double[] weights;

    public OneOf(IList<Generator<T>> generators)
    {
      this.generators = generators;
      SetUniform();
    }

    void SetUniform()
    {
      weights = new double[generators.Count];
      for (int i = 0; i < weights.Length; i++)
      {
        weights[i] = 1.0 / (double)weights.Length;
      }
    }

    public void SetWeights(double[] weights)
    {
      this.weights = weights;
    }

    public override T Generate(Random rnd)
    {
      var ix = rnd.Next(generators.Count);
      return generators[ix].Generate(rnd);
    }


  }


  /*
  public static class Utility
  {
    static T RandomEnumValue<T>(Random rnd)
    {
      var v = Enum.GetValues(typeof(T));
      return (T)v.GetValue(rnd.Next(v.Length));
    }

  }
  */

}
