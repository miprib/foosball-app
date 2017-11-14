# foosball-app

Setup:

1. Download emgu.
2. Solution Explorer tab References -> Add References. Tada paspaudžiame "Browse" ir ten kur atsiųstas emgu bin folderyje pasirenkame visus .dll (Emgu.CV.DebuggerVisualizers pasirenkame tik naujausia). Spaudžiame "Add".
3. Solution Explorer tab Vid -> Add -> Existing Item. Tada emgu folderyje einam i bin -> x64 (jei sistema 32-bit, einam i x86). "Add" visus .dll.
4. Solution Explorer tab Vid -> Properties -> Build. Tada "Platform target" pasirinkti x64 (jei sistema 32-bit, Any CPU ir pažymime "Prefer 32-bit").
