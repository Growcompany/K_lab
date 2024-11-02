using UnityEngine;
using UnityEngine.UI;

public class WeatherToggleController : MonoBehaviour
{
    public Toggle snowToggle;
    public Toggle rainToggle;
    public GameObject snowPrefab;
    public GameObject rainPrefab;
    public Transform vehicleTransform; // ������ Transform�� ����
    public AudioSource rainAudioSource; // �� �Ҹ� AudioSource

    private GameObject currentEffect;

    private void Start()
    {
        snowToggle.onValueChanged.AddListener(delegate { ToggleSnowEffect(snowToggle.isOn); });
        rainToggle.onValueChanged.AddListener(delegate { ToggleRainEffect(rainToggle.isOn); });

        // �ʱ⿡�� �� �Ҹ��� ���� ���·� ����
        if (rainAudioSource != null)
        {
            rainAudioSource.Stop();
        }
    }

    private void Update()
    {
        // ������ �̵��� �� ���� Ȱ��ȭ�� ȿ���� ��ġ�� ������Ʈ
        if (currentEffect != null)
        {
            currentEffect.transform.position = vehicleTransform.position;
        }
    }

    private void ToggleSnowEffect(bool isOn)
    {
        if (isOn)
        {
            if (currentEffect != null) Destroy(currentEffect);
            currentEffect = Instantiate(snowPrefab, vehicleTransform.position, Quaternion.identity);
            rainToggle.isOn = false;

            // �� �Ҹ� ����
            if (rainAudioSource != null && rainAudioSource.isPlaying)
            {
                rainAudioSource.Stop();
            }
        }
        else if (currentEffect && currentEffect.name.Contains("Snow"))
        {
            Destroy(currentEffect);
        }
    }

    private void ToggleRainEffect(bool isOn)
    {
        if (isOn)
        {
            if (currentEffect != null) Destroy(currentEffect);
            currentEffect = Instantiate(rainPrefab, vehicleTransform.position, Quaternion.identity);
            snowToggle.isOn = false;

            // �� �Ҹ� ���
            if (rainAudioSource != null && !rainAudioSource.isPlaying)
            {
                rainAudioSource.Play();
            }
        }
        else if (currentEffect && currentEffect.name.Contains("Rain"))
        {
            Destroy(currentEffect);

            // �� �Ҹ� ����
            if (rainAudioSource != null && rainAudioSource.isPlaying)
            {
                rainAudioSource.Stop();
            }
        }
    }
}
