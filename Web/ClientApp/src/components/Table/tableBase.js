/**
 * 定义通用表格管理混入对象
 */
export const TableBaseMixin = {
  props: {
    fields: {// table的所有字段
      type: Array,
      default: () => ([])
    },
    rows: {// table的所有数据
      type: Array,
      default: () => ([])
    },
    showTableIndex: {// 是否显示序号
      type: Boolean,
      default: true
    },
    multiSelect: {// 是否是多选
      type: Boolean,
      default: false
    },
    highlightCurrentRow: {
      type: Boolean,
      default: true
    }
  },
  data() {
    // 读取通用配置
    const config = this.$config
    return {
      componentSize: config.componentSize(),
      tableRefName: 'table',
      currentRow: {}, // 当前选择行，对于单选
      selection: [], // 当前选择行，对于多选
    }
  },
  mounted() {
  },
  methods: {
    emitEventHandler(event) { // 对所有table的事件进行监听，并向父组件抛事件，
      console.log(event)
      if (['pagination-size-change', 'pagination-current-change', 'pagination-next-click', 'pagination-prev-click'].indexOf(event) > -1) {
        // 如果是分页变化，抛出search事件，这个在snailPageTable里用到
        this.$emit('search')
      }
      if (event === 'row-click') { // 点击
        console.log('row-click' + JSON.stringify(arguments))
        var table = this.$refs[this.tableRefName]
        table.toggleRowSelection(Array.from(arguments)[1])
      }
      if (event === 'current-change') { // 单选点击
        this.currentRow = Array.from(arguments)[1]
      }

      if (event === 'selection-change') { // 多选点击
        this.selection = Array.from(arguments)[1]
      }

      this.$emit(event, ...Array.from(arguments).slice(1))// 所有事件往上抛，让外部也能监听
    }
  }
}
