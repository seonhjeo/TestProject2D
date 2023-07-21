
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BigInt
{
    public class Tester : MonoBehaviour
    {
        [SerializeField] private Button add;
        [SerializeField] private Button multiply;
        [SerializeField] private TMP_Text text;

        private BigInteger _value;

        private void OnEnable()
        {
            add.onClick.AddListener(AddValue);
            multiply.onClick.AddListener(MultiplyValue);
        }

        // Start is called before the first frame update
        void Start()
        {
            _value = new BigInteger(3795683);
            SetText();
        }

        private void OnDisable()
        {
            add.onClick.RemoveAllListeners();
            multiply.onClick.RemoveAllListeners();
        }


        public void AddValue()
        {
            _value += 1;
            SetText();
        }

        public void MultiplyValue()
        {
            _value *= 10;
            SetText();
        }

        private void SetText()
        {
            text.text = _value.ToString();
        }
    }
}

