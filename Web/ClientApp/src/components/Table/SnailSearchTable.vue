<template>
  <div style="height:100%;display:flex;flex-direction: column">
    <div>
      <!--默认功能菜单 -->
      <slot name="oper">
      </slot>
      <!-- 查询条件 -->
      <slot v-if="showSearch" name="search">
        <snail-search-form v-show="searchFields.length>0" ref="searchForm" :fields="searchFields" :rules="searchRules" @search="search" />
      </slot>
    </div>

    <div style="flex:1;">
      <!-- table分页 -->
      <el-table
        ref="table"
        :data="tableDatas"
        :highlight-current-row="highlightCurrentRow"
        @current-change="(currentRow)=>emitEventHandler('current-change',currentRow)"
        @selection-change="(selecttion)=>emitEventHandler('selection-change',selecttion)"
        @row-click="(row, column, event)=>emitEventHandler('row-click',row, column, event)"
      >
        <el-table-column v-if="multiSelect" type="selection"></el-table-column>
        <el-table-column v-if="showTableIndex" type="index" width="50">
          <template slot="header">
            序号
          </template>
        </el-table-column>
        <template v-for="(field,index) in fields">
          <el-table-column :key="index" :prop="field.name" :label="field.label" v-bind="field">
            <!-- 如果field的slotName字段有值，则用外部传入的slot来替换column里的template，否则用默认的 -->
            <template slot-scope="scope">
              <slot v-if="field.slotName" :name="field.slotName" :row="scope.row"></slot>
              <span v-else-if="field.formatter">{{ field.formatter(scope.row,scope.column, scope.row[field.name], scope.$index) }}</span>
              <span v-else-if="field.type==='select' && field.keyValues">{{ $util.keyValueFormart(field.keyValues, scope.row[field.name]) }}</span>
              <span v-else>{{ scope.row[field.name] }}</span>
            </template>
          </el-table-column>
        </template>
      </el-table>
    </div>

  </div>
</template>

<script>
import { TableBaseMixin } from './tableBase.js'
export default {
  mixins: [TableBaseMixin],
  props: {
    showSearch: {
      type: Boolean,
      default: true
    },
    autoSearch: {
      type: Boolean,
      default: false
    },
    searchApi: {
      type: String,
      default: () => ('')
    },
    searchFields: {
      type: Array,
      default: () => ([])
    },
    beforeSearch: {
      type: Function,
      default: () => { }
    },
    searchRules: {
      type: Object,
      default: () => ({})
    }
  },
  data() {
    return {
      tableDatas: [],
      visible: false,
      pagination: { pageIndex: 1, pageSize: 15, total: 0 },
      loading: false
    }
  },
  computed: {

  },
  created() {

  },
  mounted() {
    if (this.autoSearch) {
      this.search()
    }
  },
  methods: {
    emitEventHandler(event) { // 对所有table的事件进行监听，并向父组件抛事件，
      this.$emit(event, ...Array.from(arguments).slice(1))// 所有事件往上抛，让外部也能监听
    },
    search() {
      this.$refs.searchForm.validate(valid => {
        if (valid) {
          var serachForm = this.$refs.searchForm.formData
          if (typeof this.beforeSearch === 'function') {
            var isContinue = this.beforeSearch(serachForm)
            if (isContinue === false) {
              return
            }
          }
          var queryData = Object.assign({}, serachForm)
          this.loading = true
          this.$api[this.searchApi](queryData).then(res => {
            this.tableDatas = res.data
          }).finally(() => {
            this.loading = false
          })
        }
      })
    }
  }
}
</script>
