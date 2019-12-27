export const formProps = {
    size: {
        type: String,
        default: 'mini',
        validator: sizeValidator
    },
    showResetBtn: {
        type: Boolean,
        default: true
    },
    inline: {
        type: Boolean,
        default: true
    },
    rules: {
        type: Object,
        default: () => {
            return {};
        }
    },
    fuzzy: {
        type: Boolean,
        default: false
    },
    labelWidth: Number,
    itemWidth: Number,
    submitHandler: Function,
    submitLoading: {
        type: Boolean,
        default: false
    },
    submitBtnText: {
        type: String,
        default: 'common.search'
    },
    resetBtnText: {
        type: String,
        default: 'common.reset'
    },
    resetBtnCallback: Function,
    formValueChange: Function,
    valueKey: {
        type: String,
        default: 'key'
    },
    labelKey: {
        type: String,
        default: 'value'
    },
    forms: {
        type: Array,
        required: true
    }
};

function sizeValidator(value) {
    const methodTypes = ['large', 'small', 'mini'];
    const valid = methodTypes.indexOf(value.toLowerCase()) !== -1 || value === '';
    if (!valid) {
        throw new Error("Size must be one of ['large', 'small', 'mini']");
    }
    return valid;
}
