const props = {
    data: Array,
    border: {
        type: Boolean,
        default: true,
    },
    size: {
        type: String,
        default: 'mini'
    },
    stripe: {
        type: Boolean,
        default: true,
    },
    height: {
        type: String || Number,
        default: '100%'
    },
    maxHeight: [String, Number],
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
        default: true,
    },
    currentRowKey: [String, Number],
    rowClassName: [String, Function],
    rowStyle: [String, Function],
    cellStyle: [Object, Function],
    rowKey: [String, Function],
    emptyText: String,
    defaultExpandAll: Boolean,
    expandRowKeys: Array,
    defaultSort: Object,
    tooltipEffect: String,
    showSummary: Boolean,
    sumText: String,
    summaryMethod: Function,
    // 以下为非el-table自带属性
    showTableIndex: {
        type: Boolean,
        default: true
    },
    columns: {
        type: Array,
        required: true
    },
};

export default props;