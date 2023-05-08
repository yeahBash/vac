namespace Branch.DividedParts
{
    public class ShrinkingPart : DividedPartBase
    {
        public float ShrinkingSpeed = 4f;

        protected override void DoStep(float deltaTime)
        {
            Shrink(deltaTime);
        }

        private void Shrink(float deltaTime)
        {
            Growing.ChangeHeight(Growing.Length - deltaTime * ShrinkingSpeed);

            if (Growing.Length < 0) DestroyPart();
        }
    }
}