<template>
    <el-table
        v-loading.lock="loading"
        ref="table"
        :data="data"
        :border="border"
        :size="size"
        :stripe="stripe"
        :height="height"
        :max-height="maxHeight"
        :fit="fit"
        :show-header="showHeader"
        :highlight-current-row="highlightCurrentRow"
        :current-row-key="currentRowKey"
        :row-class-name="rowClassName"
        :row-style="rowStyle"
        :row-ket="rowKey"
        :empty-text="emptyText"
        :default-expand-all="defaultExpandAll"
        :expand-row-keys="expandRowKeys"
        :default-sort="defaultSort"
        :tooltip-effect="tooltipEffect"
        :show-summary="showSummary"
        :sum-text="sumText"
        :cell-style="cellStyle"
        :summary-method="summaryMethod"
        @select="(selection, row) => emitEventHandler('select', selection, row)"
        @select-all="selection => emitEventHandler('select-all', selection)"
        @selection-change="selection => emitEventHandler('selection-change', selection)"
        @cell-mouse-enter="(row, column, cell, event) => emitEventHandler('cell-mouse-enter', row, column, cell, event)"
        @cell-mouse-leave="(row, column, cell, event) => emitEventHandler('cell-mouse-leave', row, column, cell, event)"
        @cell-click="(row, column, cell, event) => emitEventHandler('cell-click', row, column, cell, event)"
        @cell-dblclick="(row, column, cell, event) => emitEventHandler('cell-dblclick', row, column, cell, event)"
        @row-click="(row, event, column) => emitEventHandler('row-click', row, event, column)"
        @row-dblclick="(row, event) => emitEventHandler('row-dblclick', row, event)"
        @row-contextmenu="(row, event) => emitEventHandler('row-contextmenu', row, event)"
        @header-click="(column, event) => emitEventHandler('header-click', column, event)"
        @sort-change="args => emitEventHandler('sort-change', args)"
        @filter-change="filters => emitEventHandler('filter-change', filters)"
        @current-change="(currentRow, oldCurrentRow) => emitEventHandler('current-change', currentRow, oldCurrentRow)"
        @header-dragend="(newWidth, oldWidth, column, event) => emitEventHandler('header-dragend', newWidth, oldWidth, column, event)"
        @expand-change="(row, expanded) => emitEventHandler('expand-change', row, expanded)"
    >
        <slot name="prepend"/>
        <el-table-column v-if="showTableIndex" type="index" width="50"></el-table-column>
        <template v-for="(column, columnIndex) in columns">
            <el-table-column
                :key="columnIndex"
                :column-key="column.columnKey"
                :prop="column.prop"
                :label="column.label"
                :width="column.minWidth ? '-' : (column.width || 'auto')"
                :minWidth="column.minWidth || column.width || 'auto'"
                :fixed="column.fixed"
                :render-header="column.renderHeader"
                :sortable="column.sortable"
                :sort-by="column.sortBy"
                :sort-method="column.method"
                :resizable="column.resizable"
                :formatter="column.formatter"
                :show-overflow-tooltip="column.showOverflowTooltip"
                :align="column.align || 'center'"
                :header-align="column.headerAlign || column.align"
                :class-name="column.className"
                :label-class-name="column.labelClassName"
                :selectable="column.selectable"
                :reserve-selection="column.reserveSelection"
                :filters="column.filters"
                :filter-placement="column.filterPlacement"
                :filter-multiple="column.filterMultiple"
                :filter-method="column.filterMethod"
                :filtered-value="column.filteredValue"
                v-if="column.type === undefined"
            >
                <template slot-scope="scope" :scope="newSlotScope ? 'scope' : false">
                    <span
                        v-if="column.filter"
                    >{{ Vue.filter(column['filter'])(scope.row[column.prop]) }}</span>
                    <span v-else-if="column.slotName">
                        <slot :name="column.slotName" :row="scope.row" :$index="scope.$index"/>
                    </span>
                    <span v-else-if="column.render">{{ column.render(scope.row) }}</span>
                    <span
                        v-else-if="column.formatter"
                    >{{ column.formatter(scope.row, scope.column, scope.row[column.prop], scope.$index) }}</span>
                    <span v-else>{{ scope.row[column.prop] }}</span>
                </template>
            </el-table-column>
            <el-table-column v-bind="column" :key="columnIndex" v-else></el-table-column>
        </template>
        <slot name="append"/>
    </el-table>
</template>

<script>
import props from './props';
export default {
    name: 'ExTable',
    props,
    data() {
        return {
            loading: false
        };
    },
    methods: {
        emitEventHandler(event) {
            this.$emit(event, ...Array.from(arguments).slice(1));
        }
    }
};
</script>
