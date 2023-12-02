using Meadow.Peripherals.Speakers;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace YourNamespace
{
    public class HalloweenSounds
    {
        private readonly IToneGenerator toneGenerator;
        private readonly int defaultDuration = 100;
        private readonly int defaultPause = 50;

        public HalloweenSounds(IToneGenerator toneGenerator)
        {
            this.toneGenerator = toneGenerator;
        }

        public async Task PlayCreakyDoor()
        {
            await toneGenerator.PlayTone(new Frequency(220), TimeSpan.FromMilliseconds(defaultDuration));
            await Task.Delay(TimeSpan.FromMilliseconds(defaultPause));
            await toneGenerator.PlayTone(new Frequency(180), TimeSpan.FromMilliseconds(defaultDuration));
            await Task.Delay(TimeSpan.FromMilliseconds(defaultPause));
            await toneGenerator.PlayTone(new Frequency(220), TimeSpan.FromMilliseconds(defaultDuration));
        }

        public async Task PlayGhostlyMoan()
        {
            await toneGenerator.PlayTone(new Frequency(300), TimeSpan.FromMilliseconds(defaultDuration));
            await Task.Delay(TimeSpan.FromMilliseconds(defaultPause));
            await toneGenerator.PlayTone(new Frequency(250), TimeSpan.FromMilliseconds(defaultDuration));
        }

        public async Task PlaySpookyMusic()
        {
            await toneGenerator.PlayTone(new Frequency(220), TimeSpan.FromMilliseconds(defaultDuration));
            await Task.Delay(TimeSpan.FromMilliseconds(defaultPause));
            await toneGenerator.PlayTone(new Frequency(180), TimeSpan.FromMilliseconds(defaultDuration));
            await Task.Delay(TimeSpan.FromMilliseconds(defaultPause));
            await toneGenerator.PlayTone(new Frequency(300), TimeSpan.FromMilliseconds(defaultDuration));
        }

        public async Task PlayWitchCackle()
        {
            await toneGenerator.PlayTone(new Frequency(400), TimeSpan.FromMilliseconds(defaultDuration));
            await toneGenerator.PlayTone(new Frequency(450), TimeSpan.FromMilliseconds(defaultDuration));
            await toneGenerator.PlayTone(new Frequency(400), TimeSpan.FromMilliseconds(defaultDuration));
            await Task.Delay(TimeSpan.FromMilliseconds(defaultPause));
            await toneGenerator.PlayTone(new Frequency(500), TimeSpan.FromMilliseconds(defaultDuration));
            await toneGenerator.PlayTone(new Frequency(550), TimeSpan.FromMilliseconds(defaultDuration));
            await toneGenerator.PlayTone(new Frequency(500), TimeSpan.FromMilliseconds(defaultDuration));
        }

        public async Task PlayCreepyLaugh()
        {
            await toneGenerator.PlayTone(new Frequency(333), TimeSpan.FromMilliseconds(defaultDuration));
            await Task.Delay(TimeSpan.FromMilliseconds(defaultPause));
            await toneGenerator.PlayTone(new Frequency(444), TimeSpan.FromMilliseconds(defaultDuration));
            await Task.Delay(TimeSpan.FromMilliseconds(defaultPause));
            await toneGenerator.PlayTone(new Frequency(555), TimeSpan.FromMilliseconds(defaultDuration));
        }

        public async Task PlayEvilLaugh()
        {
            await toneGenerator.PlayTone(new Frequency(666), TimeSpan.FromMilliseconds(defaultDuration));
            await toneGenerator.PlayTone(new Frequency(777), TimeSpan.FromMilliseconds(defaultDuration));
            await toneGenerator.PlayTone(new Frequency(888), TimeSpan.FromMilliseconds(defaultDuration));
            await Task.Delay(TimeSpan.FromMilliseconds(defaultPause));
            await toneGenerator.PlayTone(new Frequency(999), TimeSpan.FromMilliseconds(defaultDuration));
        }

        public async Task PlayThunder()
        {
            await toneGenerator.PlayTone(new Frequency(120), TimeSpan.FromMilliseconds(defaultDuration));
            await toneGenerator.PlayTone(new Frequency(150), TimeSpan.FromMilliseconds(defaultDuration));
            await toneGenerator.PlayTone(new Frequency(180), TimeSpan.FromMilliseconds(defaultDuration));
            await toneGenerator.PlayTone(new Frequency(210), TimeSpan.FromMilliseconds(defaultDuration));
        }
    }
}
