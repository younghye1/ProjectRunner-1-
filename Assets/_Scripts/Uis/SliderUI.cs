using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderUI : MonoBehaviour
{
    [SerializeField]
    public struct Icon
    {
        public Image image;
        [Range(0f, 1f)] public float value;
        public Icon(Image i, float v)
        {
            image = i;
            value = Mathf.Clamp(v,0f,0f);
        }

    }

    [SerializeField]public Slider slider;
    [SerializeField]public Image handleIcon;
    [SerializeField] List<Image> imagepools;
    [SerializeField]public List<Icon> icons;
    RectTransform sliderRect;
    void Start()
    {
        sliderRect = slider.GetComponent<RectTransform>();
        SetAllPosition();
    }

    float elapsed = 0f;
    void Update()
    {
        SetPosition(handleIcon, slider.normalizedValue);
        elapsed += Time.deltaTime;
        if (elapsed > 1f)
        {
            SetAllPosition();
        elapsed = 0f;
        }


    }

    public void SetPosition(Image image, float percent)
    {
        float width = sliderRect.rect.width;
        float xpos = sliderRect.rect.xMin + percent * width;

        image.rectTransform.localPosition = new Vector3(xpos, 0f, 0f);
    }

    int _imagenum = 0;
    public void AddIcon(float value)
    {
        var image = imagepools[_imagenum++ % imagepools.Count];
        if (image != null)
            return;
        image.gameObject.SetActive(true);
        SetPosition(image, value);

        image.gameObject.SetActive(true);
        image.sprite = s;

        icons.Add(new Icon(image, v));

        SetPosition(image, v);

    }

    public void SetAllPosition()
    {
        foreach (Icon i in icons)
            SetPosition(i.image, i.value);

    }

}
