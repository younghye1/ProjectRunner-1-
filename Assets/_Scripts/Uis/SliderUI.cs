using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderUI : MonoBehaviour
{
    [System.Serializable]
    public struct Icon
    {
        public Image image;
        [Range(0f,1f)] public float value;

        public Icon(Image i, float v)
        {
            image = i;
            value = Mathf.Clamp(v, 0f, 1f);
        }
    }


    [SerializeField] Slider slider;
    [SerializeField] Image handleIcon;

    [SerializeField] List<Image> imagepools;
    [SerializeField] List<Icon> icons;

    private RectTransform sliderRect;
    void Start()
    {
        sliderRect = slider.GetComponent<RectTransform>();
    }

    float elapsed = 0f;
    void Update()
    {
        if ( GameManager.IsPlaying == false || GameManager.IsGameover == true )
            return;

        SetPosition(handleIcon, slider.normalizedValue);

        elapsed += Time.deltaTime;
        if ( elapsed > 1f)
        {
            SetAllPosition();
            elapsed = 0f;
        }
    }

    int _imagenum = 0;
    public void AddIcon(Sprite s, float v)
    {
        if (s == null)
            return;

        Image image = imagepools[_imagenum++ % imagepools.Count];
        if (image == null)
        {
            Debug.LogWarning("사용할 수 있는 Image Pool 없음 !");
            return;
        }

        image.gameObject.SetActive(true);
        image.sprite = s;

        icons.Add(new Icon(image, v));

        SetPosition(image, v);
    }

    public void SetPosition(Image image, float percent)
    {
        float width = sliderRect.rect.width;
        float xpos = sliderRect.rect.xMin + percent * width;

        image.rectTransform.localPosition = new Vector3(xpos, 0f, 0f);
    }

    public void SetAllPosition()
    {
        foreach( Icon i in icons )
            SetPosition(i.image, i.value);
    }
}
