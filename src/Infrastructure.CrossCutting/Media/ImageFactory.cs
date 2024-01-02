namespace Infrastructure.CrossCutting.Media
{
    public static class ImageFactory
    {
        public static byte[] CreateTransparentGif() => Convert.FromBase64String("R0lGODlhAQABAIAAAAUEBAAAACwAAAAAAQABAAACAkQBADs=");
    }
}