const props = {
    // Element UI Table attributes,以下参数说明参考element文档
    height: {
        type: String || Number,
        default: '100%'
    },
    maxHeight: [String, Number],
    size: {
        type: String,
        default: 'mini'
    },
    stripe: {
        type: Boolean,
        default: true
    },
    border: {
        type: Boolean,
        default: true
    },
    fit: {
        type: Boolean,
        default: true
    },
    showHeader: {
        type: Boolean,
        default: true
    },
    highlightCurrentRow: {
        type: Boolean,
        default: true
    },
    currentRowKey: [String, Number],
    rowClassName: [String, Function],
    rowStyle: [String, Function],
    rowKey: [String, Function],
    emptyText: String,
    defaultExpandAll: Boolean,
    expandRowKeys: Array,
    defaultSort: Object,
    tooltipEffect: String,
    showOverflowTooltip: {
        type: Boolean,
        default: true
    },
    showSummary: Boolean,
    sumText: String,
    summaryMethod: Function,
    spanMethod: Function,
    // custom attributes
    fetch: {
        type: Function
    },
    listField: {
        type: String,
        default: 'data.items'
    },
    totalField: {
        type: String,
        default: 'totalItemCount'
    },
    params: {
        type: Object,
        default: () => {
            return {};
        }
    },
    autoLoad: {
        type: Boolean,
        default: true
    },
    type: {
        type: String,
        default: 'remote',
        validator(value) {
            const types = ['remote', 'local'];
            const validType = types.indexOf(value) !== -1;
            if (!validType) {
                throw new Error(`Invalid type of '${value}', please set one type of 'remote' or 'local'.`);
            }
            return validType;
        }
    },
    data: {
        type: Array
    },
    dataHandler: {
        type: Function
    },
    columns: {
        type: Array,
        required: true
    },
    showPagination: {
        type: Boolean,
        default: true
    },

    formOptions: {
        type: Object
    },
    pageSizes: {
        type: Array,
        default: () => {
            // eslint-disable-next-line
            return [15, 50, 100];
        }
    },
    paginationLayout: {
        type: String,
        default: 'total, prev, pager, next, jumper, sizes'
    },
    pageIndexKey: {
        type: String,
        default: 'PageNumber'
    },
    pageSizeKey: {
        type: String,
        default: 'pageSize'
    },
    paramsMethod: Function,
    fetchHandle: Function
};

export default props;
