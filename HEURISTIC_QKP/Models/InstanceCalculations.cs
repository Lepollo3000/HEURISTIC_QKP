
namespace HEURISTIC_QKP.Models
{
    public class InstanceCalculations
    {
        public InstanceRelation[] Relations { get; set; } = null!;

        public InstanceCalculations(Instance instance)
        {
            float[] listRelationWeightProfit = GetInstanceRWP(instance.LinearCoeficients);
            int[] listRelationProductProduct = GetInstanceRPP(instance.QuadraticCoeficients);
            float[] listRelationOfRelations = GetInstanceRR(instance.NumberCoeficients, listRelationWeightProfit, listRelationProductProduct);

            InstanceRelation[] instanceRelations = new InstanceRelation[instance.NumberCoeficients];

            for (int i = 0; i < instance.LinearCoeficients.Count(); i++)
            {
                float relationOfRelations = listRelationProductProduct[i] / listRelationWeightProfit[i];

                InstanceRelation calculation = new InstanceRelation()
                {
                    LinearCoeficient = instance.LinearCoeficients[i],
                    RelationWeightProfit = listRelationWeightProfit[i],
                    RelationProductProduct = listRelationProductProduct[i],
                    RelationOfRelations = listRelationOfRelations[i]
                };

                instanceRelations[i] = calculation;
            }

            instanceRelations = instanceRelations.OrderBy(c => c.RelationOfRelations).ToArray();

            Relations = instanceRelations;
        }

        private int[] GetInstanceRPP(QuadraticCoeficient[,] quadraticCoeficients)
        {
            int[] relationProductProduct = new int[quadraticCoeficients.GetLength(0)];

            for (int i = 0; i < quadraticCoeficients.GetLength(0); i++)
            {
                int sum = 0;

                for (int j = 0; j < quadraticCoeficients.GetLength(1); j++)
                {
                    sum += quadraticCoeficients[i, j].Value;
                }

                relationProductProduct[i] = sum;
            }

            return relationProductProduct;
        }

        private float[] GetInstanceRWP(LinearCoeficient[] linearCoeficients)
        {
            return linearCoeficients.Select(cl => (float)cl.Value / (float)cl.Weight).ToArray();
        }

        private float[] GetInstanceRR(int numberCoeficients, float[] relationsWeightProfit, int[] relationsProductProduct)
        {
            float[] relationsOfRelations = new float[numberCoeficients];

            for (int i = 0; i < numberCoeficients; i++)
            {
                relationsOfRelations[i] = relationsProductProduct[i] / relationsWeightProfit[i];
            }

            return relationsOfRelations;
        }
    }
}
