<template>
  <el-table
    ref="table"
    :data="rows"
    :highlight-current-row="highlightCurrentRow"
    @current-change="(currentRow)=>emitEventHandler('current-change',currentRow)"
    @selection-change="(selecttion)=>emitEventHandler('selection-change',selecttion)"
    @row-click="(row, column, event)=>emitEventHandler('row-click',row, column, event)"
  >
    <el-table-column v-if="multiSelect" type="selection"></el-table-column>
    <el-table-column v-if="showTableIndex" type="index" width="50">
      <template slot="header">序号</template>
    </el-table-column>
    <template v-for="(field,index) in fields">
      <el-table-column :key="index" :prop="field.name" :label="field.label" v-bind="field">
        <!-- 如果field的slotName字段有值，则用外部传入的slot来替换column里的template，否则用默认的 -->
        <template slot-scope="scope">
          <slot v-if="field.slotName" :name="field.slotName" :row="scope.row"></slot>
          <span v-else-if="field.formatter">{{ field.formatter(scope.row,scope.column, scope.row[field.name], scope.$index) }}</span>
          <span v-else>{{ scope.row[field.name] }}</span>
        </template>
      </el-table-column>
    </template>
  </el-table>
</template>

<script>
import { TableBaseMixin } from './tableBase.js'
export default {
  name: 'SnailTable',
  mixins: [TableBaseMixin]

}
</script>
