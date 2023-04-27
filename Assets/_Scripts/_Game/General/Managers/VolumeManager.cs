using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Scripts._Game.General.Managers{
    
    public class VolumeManager : Singleton<VolumeManager>
    {
        private Volume _globalVolume;

        [Header("Bloom")]
        private Bloom _globalVolumeBloom;
        private MinFloatParameter _bloomDefaultIntensity;

        [Header("Film grain")]
        private FilmGrain _globalVolumeFilmGrain;
        private FilmGrainLookupParameter _filmGrainDefaultType;
        private ClampedFloatParameter _filmGrainDefaultIntensity;

        [Header("Vignette")]
        private Vignette _globalVolumeVignette;
        private ColorParameter _vignetteColour;
        private Vector2Parameter _vignetteCenter;
        private ClampedFloatParameter _vignetteIntensity;
        private ClampedFloatParameter _vignetteSmoothness;
        private BoolParameter _vignetteRounded;


        protected new void Awake()
        {
            base.Awake();

            _globalVolume = GetComponent<Volume>();

            if (_globalVolume != null)
            {
                if (_globalVolume.profile.TryGet<Bloom>(out _globalVolumeBloom))
                {
                    _bloomDefaultIntensity = _globalVolumeBloom.intensity;
                }

                if (_globalVolume.profile.TryGet<FilmGrain>(out _globalVolumeFilmGrain))
                {
                    _filmGrainDefaultType = _globalVolumeFilmGrain.type;
                    _filmGrainDefaultIntensity = _globalVolumeFilmGrain.intensity;
                }

                if (_globalVolume.profile.TryGet<Vignette>(out _globalVolumeVignette))
                {
                    _vignetteColour = _globalVolumeVignette.color;
                    _vignetteCenter = _globalVolumeVignette.center;
                    _vignetteIntensity = _globalVolumeVignette.intensity;
                    _vignetteSmoothness = _globalVolumeVignette.smoothness;
                    _vignetteRounded = _globalVolumeVignette.rounded;
                }
            }
        }
    }
    
}
