namespace CSharpSynth.Midi
{
    public class MidiTrack
    {
        //--Variables
        public uint NotesPlayed;
        public ulong TotalTime;
        public byte[] Programs;
        public byte[] DrumPrograms;
        public MidiEvent[] MidiEvents 
        {
            get { return _modifiedMidiEvents; }
            set { 
                _originalMidiEvents = value;
                _modifiedMidiEvents = value;
            }
        }
        //--Public Properties
        public int EventCount
        {
            get { return MidiEvents.Length; }
        }

        private MidiEvent[] _originalMidiEvents;
        private MidiEvent[] _modifiedMidiEvents;
        //--Public Methods
        public MidiTrack()
        {
            NotesPlayed = 0;
            TotalTime = 0;
        }
        public bool ContainsProgram(byte program)
        {
            for (int x = 0; x < Programs.Length; x++)
            {
                if (Programs[x] == program)
                    return true;
            }
            return false;
        }
        public bool ContainsDrumProgram(byte drumprogram)
        {
            for (int x = 0; x < DrumPrograms.Length; x++)
            {
                if (DrumPrograms[x] == drumprogram)
                    return true;
            }
            return false;
        }

        public void ApplyFilter(MIDITrackFilter filter) {
            _modifiedMidiEvents = filter.FilterMidiEvents(_originalMidiEvents);
        }
    }
}
