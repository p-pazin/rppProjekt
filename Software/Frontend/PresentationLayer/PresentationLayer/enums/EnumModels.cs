using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.enums
{
    public class EnumModels
    {
        public enum BMWModel
        {
            [Description("X5")]
            X5,

            [Description("3 Series")]
            Series3,

            [Description("5 Series")]
            Series5,

            [Description("7 Series")]
            Series7,

            [Description("i8")]
            i8,

            [Description("M3")]
            M3,

            [Description("M4")]
            M4,

            [Description("iX")]
            iX
        }

        public enum AudiModel
        {
            [Description("A3")]
            A3,

            [Description("A4")]
            A4,

            [Description("A6")]
            A6,

            [Description("A8")]
            A8,

            [Description("Q5")]
            Q5,

            [Description("Q7")]
            Q7,

            [Description("Q8")]
            Q8,

            [Description("R8")]
            R8
        }

        public enum MercedesModel
        {
            [Description("C-Class")]
            CClass,

            [Description("E-Class")]
            EClass,

            [Description("S-Class")]
            SClass,

            [Description("A-Class")]
            AClass,

            [Description("GLC")]
            GLC,

            [Description("GLE")]
            GLE,

            [Description("GLS")]
            GLS,

            [Description("EQC")]
            EQC
        }

        public enum SkodaModel
        {
            [Description("Octavia")]
            Octavia,

            [Description("Superb")]
            Superb,

            [Description("Fabia")]
            Fabia,

            [Description("Kodiaq")]
            Kodiaq,

            [Description("Karoq")]
            Karoq
        }

        public enum RenaultModel
        {
            [Description("Clio")]
            Clio,

            [Description("Captur")]
            Captur,

            [Description("Megane")]
            Megane,

            [Description("Scenic")]
            Scenic,

            [Description("Twingo")]
            Twingo
        }

        public enum VolkswagenModel
        {
            [Description("Golf")]
            Golf,

            [Description("Passat")]
            Passat,

            [Description("Jetta")]
            Jetta,

            [Description("Tiguan")]
            Tiguan,

            [Description("Touran")]
            Touran,

            [Description("Arteon")]
            Arteon
        }

        public enum OpelModel
        {
            [Description("Astra")]
            Astra,

            [Description("Corsa")]
            Corsa,

            [Description("Insignia")]
            Insignia,

            [Description("Grandland")]
            Grandland,

            [Description("Mokka")]
            Mokka
        }

        public enum FordModel
        {
            [Description("Fiesta")]
            Fiesta,

            [Description("Focus")]
            Focus,

            [Description("Mondeo")]
            Mondeo,

            [Description("Kuga")]
            Kuga,

            [Description("Puma")]
            Puma
        }

        public enum PeugeotModel
        {
            [Description("208")]
            Model208,

            [Description("3008")]
            Model3008,

            [Description("5008")]
            Model5008,

            [Description("508")]
            Model508,

            [Description("Partner")]
            Partner
        }

        public enum HondaModel
        {
            [Description("Civic")]
            Civic,

            [Description("Accord")]
            Accord,

            [Description("CR-V")]
            CRV,

            [Description("HR-V")]
            HRV,

            [Description("Jazz")]
            Jazz
        }

        public enum ToyotaModel
    {
        [Description("Corolla")]
        Corolla,

        [Description("Yaris")]
        Yaris,

        [Description("Camry")]
        Camry,

        [Description("RAV4")]
        RAV4,

        [Description("Hilux")]
        Hilux
    }

    public enum FiatModel
    {
        [Description("500")]
        Model500,

        [Description("Panda")]
        Panda,

        [Description("Tipo")]
        Tipo,

        [Description("Doblo")]
        Doblo,

        [Description("500L")]
        Model500L
    }

    public enum CitroenModel
    {
        [Description("C3")]
        C3,

        [Description("C4")]
        C4,

        [Description("C5 Aircross")]
        C5Aircross,

        [Description("Berlingo")]
        Berlingo
    }

    public enum FerrariModel
    {
        [Description("Portofino")]
        Portofino,

        [Description("Roma")]
        Roma,

        [Description("812 Superfast")]
        Superfast,

        [Description("SF90 Stradale")]
        SF90Stradale
    }

    public enum LamborghiniModel
    {
        [Description("Huracán")]
        Huracán,

        [Description("Aventador")]
        Aventador,

        [Description("Urus")]
        Urus
    }

    public enum PorscheModel
    {
        [Description("911")]
        Model911,

        [Description("Cayenne")]
        Cayenne,

        [Description("Macan")]
        Macan,

        [Description("Taycan")]
        Taycan
    }

    public enum AstonMartinModel
    {
        [Description("Vantage")]
        Vantage,

        [Description("DB11")]
        DB11,

        [Description("DBS Superleggera")]
        DBSuperleggera,

        [Description("Valhalla")]
        Valhalla
    }

    public enum JaguarModel
    {
        [Description("XE")]
        XE,

        [Description("XJ")]
        XJ,

        [Description("F-Type")]
        FType,

        [Description("F-Pace")]
        FPace
    }

    public enum LandRoverModel
    {
        [Description("Defender")]
        Defender,

        [Description("Discovery")]
        Discovery,

        [Description("Range Rover")]
        RangeRover,

        [Description("Range Rover Sport")]
        RangeRoverSport
    }

    public enum ChryslerModel
    {
        [Description("Pacifica")]
        Pacifica,

        [Description("Voyager")]
        Voyager
    }

    public enum DodgeModel
    {
        [Description("Charger")]
        Charger,

        [Description("Challenger")]
        Challenger,

        [Description("Durango")]
        Durango
    }

    public enum BuickModel
    {
        [Description("Encore")]
        Encore,

        [Description("Envision")]
        Envision,

        [Description("LaCrosse")]
        LaCrosse
    }

    public enum ChevroletModel
    {
        [Description("Cruze")]
        Cruze,

        [Description("Malibu")]
        Malibu,

        [Description("Equinox")]
        Equinox,

        [Description("Camaro")]
        Camaro
    }

    public enum NissanModel
    {
        [Description("Micra")]
        Micra,

        [Description("Altima")]
        Altima,

        [Description("370Z")]
        Model370Z,

        [Description("Juke")]
        Juke,

        [Description("Qashqai")]
        Qashqai
    }

    public enum MitsubishiModel
    {
        [Description("Lancer")]
        Lancer,

        [Description("Outlander")]
        Outlander,

        [Description("Pajero")]
        Pajero
    }

    public enum KiaModel
    {
        [Description("Soul")]
        Soul,

        [Description("Ceed")]
        Ceed,

        [Description("Sportage")]
        Sportage,

        [Description("Stinger")]
        Stinger
    }

    public enum HyundaiModel
    {
        [Description("i10")]
        i10,

        [Description("i30")]
        i30,

        [Description("Tucson")]
        Tucson,

        [Description("Santa Fe")]
        SantaFe
    }

    public enum SubaruModel
    {
        [Description("Impreza")]
        Impreza,

        [Description("Outback")]
        Outback,

        [Description("Forester")]
        Forester
    }

    public enum AlfaRomeoModel
    {
        [Description("Giulia")]
        Giulia,

        [Description("Stelvio")]
        Stelvio,

        [Description("Tonale")]
        Tonale
    }

    public enum SeatModel
    {
        [Description("Leon")]
        Leon,

        [Description("Ateca")]
        Ateca,

        [Description("Ibiza")]
        Ibiza
    }

    public enum MazdaModel
    {
        [Description("3")]
        Model3,

        [Description("6")]
        Model6,

        [Description("CX-5")]
        CX5,

        [Description("MX-5")]
        MX5
    }

    public enum SaabModel
    {
        [Description("9-3")]
        Model9_3,

        [Description("9-5")]
        Model9_5
    }

    public enum TeslaModel
    {
        [Description("Model S")]
        ModelS,

        [Description("Model 3")]
        Model3,

        [Description("Model X")]
        ModelX,

        [Description("Model Y")]
        ModelY
    }

    public enum LincolnModel
    {
        [Description("Navigator")]
        Navigator,

        [Description("Aviator")]
        Aviator
    }

    public enum CadillacModel
    {
        [Description("Escalade")]
        Escalade,

        [Description("XT5")]
        XT5
    }

    public enum GenesisModel
    {
        [Description("G70")]
        G70,

        [Description("G80")]
        G80,

        [Description("GV80")]
        GV80
    }

    public enum MiniModel
    {
        [Description("Cooper")]
        Cooper,

        [Description("Clubman")]
        Clubman
    }

    public enum SmartModel
    {
        [Description("Fortwo")]
        Fortwo,

        [Description("Forfour")]
        Forfour
    }

    public enum RivianModel
    {
        [Description("R1T")]
        R1T,

        [Description("R1S")]
        R1S
    }

}
}
