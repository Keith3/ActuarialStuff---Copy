Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Drawing.StringFormatFlags
Imports System.Math
Imports ActuarialLib

Public Class PrintTable
	Inherits PrintDocument

	Private mTableTitle As String = "Title"
	Private mTableSubTitleH As String = "Issue Age"
	Private mTableSubTitleV As String = "Duration"
	Private mTableColHdgs As New List(Of String)
	Private mTableRowHdgs As New List(Of String)
	Private mTableRows As New List(Of String())
	Private mTableColCount As Integer = 0
	Private mTableTailSubTitle As String = "Attained Age"
	Private mTableTailRowHdgs As New List(Of String)
	Private mTableTailRows As New List(Of String)

	Private mTableFontFamily As New FontFamily("Arial")

	Private mTitleFont As Font
	Private mTitleStyle As FontStyle = FontStyle.Bold
	Private mTitleSize As Single = 12.0!
	Private mTitleHeight As Single

	Private mSubTitleFont As Font
	Private mSubTitleStyle As FontStyle = FontStyle.Bold
	Private mSubTitleSize As Single = 10.0!
	Private mSubTitleVSize As SizeF
	Private mSubTitleHHeight As Single

	Private mColWidth() As Single
	Private mRowHdgsWidth As Single
	Private mRowColHdgFont As Font
	Private mRowColHdgStyle As FontStyle = FontStyle.Bold
	Private mRowColHdgSize As Single = 8.0!
	Private mColHdgHeight As Single

	Private mDataFont As Font
	Private mDataStyle As FontStyle = FontStyle.Regular
	Private mDataSize As Single = 8.0!
	Private mDataRowHeight As Single

	Private mCurPageNum As Integer

	Private mFrmtCenterH As StringFormat
	Private mFrmtRightH As StringFormat
	Private mFrmtCenterV As StringFormat

	Private mPageArea As Rectangle
	Private mTitleArea As RectangleF
	Private mSubTitleAreaH As RectangleF
	Private mSubTitleAreaV As RectangleF
	Private mColHdgArea As RectangleF
	Private mRowHdgArea As RectangleF
	Private mDataArea As RectangleF
	Private mDataColsArea() As RectangleF

	Private mPages As New List(Of PageBounds)

	Public Sub New()

		mFrmtCenterH = New StringFormat()
		mFrmtCenterH.LineAlignment = StringAlignment.Center
		mFrmtCenterH.Alignment = StringAlignment.Center
		mFrmtCenterH.FormatFlags = NoClip Or NoWrap

		mFrmtRightH = New StringFormat()
		mFrmtRightH.LineAlignment = StringAlignment.Center
		mFrmtRightH.Alignment = StringAlignment.Far
		mFrmtRightH.FormatFlags = NoClip Or NoWrap

		mFrmtCenterV = New StringFormat(StringFormatFlags.DirectionVertical)
		mFrmtCenterV.LineAlignment = StringAlignment.Far
		mFrmtCenterV.Alignment = StringAlignment.Center
		mFrmtCenterV.FormatFlags = DirectionVertical Or NoClip Or NoWrap

	End Sub

	Public Overloads Sub TableAddRow(ByVal rowHeading As String, _
									 ByVal ParamArray row() As String)

		mTableRowHdgs.Add(rowHeading)
		mTableRows.Add(row)
		mTableColCount = Max(mTableColCount, row.GetUpperBound(0) + 1)

	End Sub

	Public Overloads Sub TableAddRow(ByVal rowHeading As String, _
									 ByVal rowFormat As String, _
									 ByVal ParamArray row() As Double)

		Dim RowUBound As Integer = row.GetUpperBound(0)
		Dim RowStr(RowUBound) As String

		For i As Integer = 0 To RowUBound
			RowStr(i) = row(i).ToString(rowFormat).Trim
		Next

		TableAddRow(rowHeading, RowStr)

	End Sub

	Public Overloads Sub TableAddRow(ByVal rowHeading As String, _
									 ByVal ParamArray row() As Double)

		Me.TableAddRow(rowHeading, "N2", row)
	End Sub

	Public Sub TableClear()
		mTableTitle = String.Empty
		mTableSubTitleH = String.Empty
		mTableSubTitleV = String.Empty
		mTableColHdgs.Clear()
		mTableRowHdgs.Clear()
		mTableRows.Clear()
		mTableColCount = 0
		mTableTailSubTitle = String.Empty
		mTableTailRowHdgs.Clear()
		mTableTailRows.Clear()
	End Sub

	Public Property TableTitle() As String
		Get
			Return mTableTitle
		End Get
		Set(ByVal Value As String)
			mTableTitle = Value
		End Set
	End Property

	Public Property TableSubTitleV() As String
		Get
			Return mTableSubTitleV
		End Get
		Set(ByVal Value As String)
			mTableSubTitleV = Value
		End Set
	End Property

	Public Property TableSubTitleH() As String
		Get
			Return mTableSubTitleH
		End Get
		Set(ByVal Value As String)
			mTableSubTitleH = Value
		End Set
	End Property

	Public Function TableColumnCount() As Integer
		Return mTableColCount
	End Function

	Public Property TableColumnHeadings() As List(Of String)
		Get
			Return mTableColHdgs
		End Get
		Set(ByVal Value As List(Of String))
			mTableColHdgs = Value
		End Set
	End Property

	Public Function TableRowCount() As Integer
		Return mTableRows.Count
	End Function

	Public Function TableRowHeadings() As List(Of String)
		Return mTableRowHdgs
	End Function

	Public Function TableRows() As List(Of String())
		Return mTableRows
	End Function

	Public Property TableTailSubTitle() As String
		Get
			Return mTableTailSubTitle
		End Get
		Set(ByVal value As String)
			mTableTailSubTitle = value
		End Set
	End Property

	Public Property TableTailRowHeadings() As List(Of String)
		Get
			Return mTableTailRowHdgs
		End Get
		Set(ByVal value As List(Of String))
			mTableTailRowHdgs = value
		End Set
	End Property

	Public Property TableTailRows() As List(Of String)
		Get
			Return mTableTailRows
		End Get
		Set(ByVal value As List(Of String))
			mTableTailRows = value
		End Set
	End Property

	Public Sub TableTailAddRow(ByVal tailRowHeading As String, _
							   ByVal tailRow As String)

		mTableTailRowHdgs.Add(tailRowHeading)
		mTableTailRows.Add(tailRow)
	End Sub

	Public Sub TableTailAddRow(ByVal rowHeading As String, _
							   ByVal row As Double, _
							   ByVal rowFormat As String)

		TableTailAddRow(rowHeading, row.ToString(rowFormat))
	End Sub

	Public Sub TableTailAddRow(ByVal rowHeading As String, _
							   ByVal row As Double)

		TableTailAddRow(rowHeading, row, "N2")
	End Sub

	Public Sub TableFontFamily(ByVal value As FontFamily)
		mTableFontFamily = value
	End Sub

	Public Sub TitleStyle(ByVal style As FontStyle, _
						  ByVal size As Single)

		mTitleStyle = style
		mTitleSize = size
	End Sub

	Public Sub SubTitleStyle(ByVal style As FontStyle, _
							 ByVal size As Single)

		mSubTitleStyle = style
		mSubTitleSize = size
	End Sub

	Public Sub RowColHdgStyle(ByVal style As FontStyle, _
							  ByVal size As Single)

		mRowColHdgStyle = style
		mRowColHdgSize = size
	End Sub

	Public Sub DataStyle(ByVal style As FontStyle, _
						 ByVal size As Single)

		mDataStyle = style
		mDataSize = size
	End Sub

	Private Structure PageBounds
		Public RowFrst As Integer
		Public RowLast As Integer
		Public ColFrst As Integer
		Public ColLast As Integer
	End Structure

	Protected Overrides Sub OnBeginPrint(ByVal e As PrintEventArgs)

		MyBase.OnBeginPrint(e)

		mCurPageNum = 0

		mTitleFont = New Font(mTableFontFamily, mTitleSize, mTitleStyle)
		mSubTitleFont = New Font(mTableFontFamily, mSubTitleSize, mSubTitleStyle)
		mRowColHdgFont = New Font(mTableFontFamily, mDataSize, mRowColHdgStyle)
		mDataFont = New Font(mTableFontFamily, mDataSize, FontStyle.Regular)

		mTitleHeight = mTitleFont.GetHeight
		mSubTitleHHeight = mSubTitleFont.GetHeight
		mColHdgHeight = mRowColHdgFont.Height
		mDataRowHeight = Max(mRowColHdgFont.GetHeight, mDataFont.GetHeight)

		ReDim mDataColsArea(mTableColCount - 1)
		ReDim mColWidth(mTableColCount - 1)

	End Sub

	Protected Overrides Sub OnPrintPage(ByVal e As PrintPageEventArgs)

		MyBase.OnPrintPage(e)

		mCurPageNum += 1

		If mCurPageNum = 1 Then
			GetTableInfo(e)
			PreparePages(e)
		End If

		If mCurPageNum <= mPages.Count Then
			DrawPage(e)
			e.HasMorePages = True
		ElseIf mTableTailRows.Count > 0 Then
			DrawTailPage(e)
			e.HasMorePages = False
		Else
			e.HasMorePages = False
		End If

	End Sub

	Protected Overrides Sub OnEndPrint(ByVal e As PrintEventArgs)
		MyBase.OnEndPrint(e)

		mTableFontFamily.Dispose()
		mTitleFont.Dispose()
		mSubTitleFont.Dispose()
		mRowColHdgFont.Dispose()
		mDataFont.Dispose()
		mFrmtCenterH.Dispose()
		mFrmtRightH.Dispose()
		mFrmtCenterV.Dispose()
	End Sub

	Private Sub GetTableInfo(ByRef e As PrintPageEventArgs)

		mPageArea = e.MarginBounds
		mPageArea.Intersect(e.PageBounds)

		mRowHdgsWidth = 0

		For i As Integer = 0 To mTableRows.Count - 1
			Dim RowHdgSize As SizeF = e.Graphics.MeasureString(mTableRowHdgs(i), mRowColHdgFont)
			mRowHdgsWidth = Max(mRowHdgsWidth, RowHdgSize.Width)
		Next

		For j As Integer = 0 To mTableColCount - 1
			If j <= mTableColHdgs.Count - 1 Then
				mColWidth(j) = e.Graphics.MeasureString(mTableColHdgs(j), mRowColHdgFont).Width
			Else
				mColWidth(j) = 0
			End If
		Next

		Dim DataWidth As Single

		For i As Integer = 0 To mTableRows.Count - 1
			For j As Integer = 0 To mTableColCount - 1
				DataWidth = e.Graphics.MeasureString(mTableRows(i)(j), mDataFont).Width
				mColWidth(j) = Max(mColWidth(j), DataWidth)
			Next
		Next

		Dim ExtraSpace As Single = e.Graphics.MeasureString("1", mDataFont).Width
		For j As Integer = 0 To mTableColCount - 1
			mColWidth(j) += ExtraSpace
		Next

		mSubTitleVSize = e.Graphics.MeasureString(mTableSubTitleV, mSubTitleFont)

		Dim HorizontalOffset As Single = mSubTitleVSize.Height + mRowHdgsWidth
		Dim VerticalOffset As Single = mTitleHeight + mSubTitleHHeight + mColHdgHeight

		mTitleArea = New RectangleF(mPageArea.Left + HorizontalOffset, _
									mPageArea.Top, _
									mPageArea.Width, _
									mTitleHeight)

		mSubTitleAreaH = New RectangleF(mPageArea.Left + HorizontalOffset, _
										mTitleArea.Bottom, _
										mPageArea.Width, _
										mSubTitleHHeight)

		mSubTitleAreaV = New RectangleF(mPageArea.Left, _
										mPageArea.Top + VerticalOffset, _
										mSubTitleVSize.Height, _
										mPageArea.Height - VerticalOffset)

		mRowHdgArea = New RectangleF(mSubTitleAreaV.Right, _
									 mPageArea.Top + VerticalOffset, _
									 mRowHdgsWidth, _
									 mPageArea.Height - VerticalOffset)

		mDataArea = New RectangleF(mPageArea.Left + HorizontalOffset, _
								   mPageArea.Top + VerticalOffset, _
								   mPageArea.Width - HorizontalOffset, _
								   mPageArea.Height - VerticalOffset)

		If mTableRows.Count > 0 AndAlso mTableColCount > 0 Then

			Dim ColLeft As Single = mPageArea.Left + HorizontalOffset

			For j As Integer = 0 To mTableColCount - 1

				mDataColsArea(j) = New RectangleF(ColLeft, _
												  mDataArea.Top, _
												  mColWidth(j), _
												  mDataRowHeight)
			Next

			mColHdgArea = New RectangleF(mPageArea.Left + HorizontalOffset, _
										 mSubTitleAreaH.Bottom, _
										 mColWidth(0), _
										 mColHdgHeight)

		End If

	End Sub

	Private Sub PreparePages(ByVal e As PrintPageEventArgs)

		mPages.Clear()

		If mTableRows.Count = 0 OrElse mTableColCount = 0 Then Return

		Dim Page As New PageBounds()
		Page.RowFrst = 0
		Page.ColFrst = 0

		Do

			Page.RowLast = Min(Page.RowFrst + CInt(Floor(mDataArea.Height / mDataRowHeight)) - 1, mTableRows.Count - 1)
			Page.ColLast = mTableColCount - 1

			Dim Width As Single = 0

			For i As Integer = Page.ColFrst To Page.ColLast
				Width += mColWidth(i)
				If Width > mDataArea.Width Then
					Page.ColLast = i - 1
					Exit For
				End If
			Next

			mPages.Add(Page)

			If Page.ColLast = mTableColCount - 1 Then
				If Page.RowLast = mTableRows.Count - 1 Then
					Exit Do
				Else
					Dim FrstRow As Integer = Page.RowLast + 1
					Page = New PageBounds()
					Page.RowFrst = FrstRow
					Page.ColFrst = 0
				End If
			Else
				Dim FrstRow As Integer = Page.RowFrst
				Dim FrstCol As Integer = Page.ColLast + 1
				Page = New PageBounds()
				Page.ColFrst = FrstCol
				Page.RowFrst = FrstRow
			End If

		Loop

		For Each p As PageBounds In mPages

			Dim LastFound As Boolean = False

			p.RowLast = Min(p.RowLast, mTableRowHdgs.Count - 1)

			If p.RowLast >= p.RowFrst Then
				Dim MaxCol As Integer = Integer.MinValue

				For i = p.RowLast To p.RowFrst Step -1
					MaxCol = Max(MaxCol, mTableRows(i).GetUpperBound(0))
					If (Not LastFound) AndAlso (mTableRows(i).GetUpperBound(0) >= p.ColFrst) Then
						p.RowLast = i
						LastFound = True
					End If
				Next

				p.ColLast = Min(p.ColLast, MaxCol)
				If Not LastFound Then
					p.ColLast = p.ColFrst - 1
					p.RowLast = p.RowFrst - 1
				End If
			Else
				p.ColLast = p.ColFrst - 1
			End If

		Next

		For i = mPages.Count - 1 To 0 Step -1
			Dim p As PageBounds = mPages(i)

			If p.RowFrst > p.RowLast OrElse p.ColFrst > p.ColLast Then
				mPages.Remove(p)
			End If
		Next

	End Sub

	Private Sub DrawPage(ByVal e As PrintPageEventArgs)

		e.Graphics.DrawString(mTableTitle, _
							  mTitleFont, _
							  Brushes.Black, _
							  mTitleArea, _
							  mFrmtCenterH)

		e.Graphics.DrawString(mTableSubTitleH, _
							  mSubTitleFont, _
							  Brushes.Black, _
							  mSubTitleAreaH, _
							  mFrmtCenterH)

		e.Graphics.DrawString(mTableSubTitleV, _
							  mSubTitleFont, _
							  Brushes.Black, _
							  mSubTitleAreaV, _
							  mFrmtCenterV)

		Dim PageBnds As PageBounds = mPages(mCurPageNum - 1)
		Dim ColLast As Integer = Min(PageBnds.ColLast, mTableColHdgs.Count - 1)
		Dim ColHdgAreaFrst As RectangleF = mColHdgArea
		ColHdgAreaFrst.Width = mColWidth(PageBnds.ColFrst)

		For j As Integer = PageBnds.ColFrst To ColLast

			e.Graphics.DrawString(mTableColHdgs(j), _
								  mRowColHdgFont, _
								  Brushes.Black, _
								  ColHdgAreaFrst, _
								  mFrmtCenterH)

			ColHdgAreaFrst.X += mColWidth(j)
		Next

		Dim RowLast As Integer = Min(PageBnds.RowLast, mTableRowHdgs.Count - 1)
		Dim mDataAreaCol As RectangleF = mDataColsArea(PageBnds.ColFrst)
		Dim RowHdgArea As RectangleF = mRowHdgArea

		For i As Integer = PageBnds.RowFrst To RowLast

			e.Graphics.DrawString(mTableRowHdgs(i), _
								  mRowColHdgFont, _
								  Brushes.Black, _
								  RowHdgArea, _
								  mFrmtRightH)

			RowHdgArea.Y += mDataRowHeight

			ColLast = Min(PageBnds.ColLast, mTableRows(i).GetUpperBound(0))

			mDataAreaCol.Width = mColWidth(PageBnds.ColFrst)

			For j As Integer = PageBnds.ColFrst To ColLast

				e.Graphics.DrawString(mTableRows(i)(j), _
									  mDataFont, _
									  Brushes.Black, _
									  mDataAreaCol, _
									  mFrmtRightH)

				mDataAreaCol.X += mColWidth(j)

			Next

			mDataAreaCol.X = mColHdgArea.Left
			mDataAreaCol.Y += mDataRowHeight

		Next

	End Sub

	Private Sub DrawTailPage(ByVal e As PrintPageEventArgs)

		Dim SubTitleTailArea As RectangleF = New RectangleF(mPageArea.Left, _
															mPageArea.Top + mTitleHeight, _
															mSubTitleVSize.Height, _
															mPageArea.Height - mTitleHeight)

		Dim RowHdgsWidth As Single = 0

		For i As Integer = 0 To mTableTailRowHdgs.Count - 1
			Dim RowHdgSize As SizeF = e.Graphics.MeasureString(mTableTailRowHdgs(i), mRowColHdgFont)
			RowHdgsWidth = Max(RowHdgsWidth, RowHdgSize.Width)
		Next

		Dim ColWidth As Single = 0
		Dim DataWidth As Single

		For i As Integer = 0 To mTableTailRows.Count - 1
			DataWidth = e.Graphics.MeasureString(mTableTailRows(i), mDataFont).Width
			ColWidth = Max(ColWidth, DataWidth)
		Next

		ColWidth += e.Graphics.MeasureString("1", mDataFont).Width

		e.Graphics.DrawString(mTableTitle, _
							  mTitleFont, _
							  Brushes.Black, _
							  mTitleArea, _
							  mFrmtCenterH)

		Dim RowCount As Integer = CInt(Floor(SubTitleTailArea.Height / mDataRowHeight))

		Dim RowHdgArea As RectangleF = New RectangleF(mPageArea.Left + mSubTitleVSize.Height, _
													  mPageArea.Top + mTitleHeight, _
													  RowHdgsWidth, _
													  mDataRowHeight)

		Dim RowDataArea As RectangleF = New RectangleF(RowHdgArea.Right, _
													   mPageArea.Top + mTitleHeight, _
													   ColWidth, _
													   mDataRowHeight)

		For j = 0 To mTableTailRows.Count - 1 Step RowCount

			e.Graphics.DrawString(mTableTailSubTitle, _
								  mSubTitleFont, _
								  Brushes.Black, _
								  SubTitleTailArea, _
								  mFrmtCenterV)

			For a As Integer = j To Min(j + RowCount - 1, mTableTailRows.Count - 1)

				e.Graphics.DrawString(mTableTailRowHdgs(a), _
									  mRowColHdgFont, _
									  Brushes.Black, _
									  RowHdgArea, _
									  mFrmtRightH)

				e.Graphics.DrawString(mTableTailRows(a), _
									  mDataFont, _
									  Brushes.Black, _
									  RowDataArea, _
									  mFrmtRightH)

				RowHdgArea.Y += mDataRowHeight
				RowDataArea.Y += mDataRowHeight

			Next


			SubTitleTailArea.X += 100
			RowHdgArea.X += 100
			RowHdgArea.Y = mPageArea.Top + mTitleHeight
			RowDataArea.X += 100
			RowDataArea.Y = mPageArea.Top + mTitleHeight

		Next

	End Sub

	Public Function GetDocument1000q(ByVal printerSettings As PrinterSettings, _
										 ByVal pageSettings As PageSettings, _
										 ByVal table As SOATable) As PrintTable

		Dim Doc As New PrintTable()
		Dim Row() As Double

		Doc.PrinterSettings = printerSettings
		Doc.DefaultPageSettings.Landscape = pageSettings.Landscape
		Doc.DefaultPageSettings.Margins = pageSettings.Margins
		Doc.TableTitle = table.Name
		Doc.TableTailSubTitle = "Attained Age"

		If table.StructureCode = TableStructure.SelectAndUltimate Then

			Doc.TableSubTitleH = "Year"
			Doc.TableSubTitleV = "Issue Age"

			For y As Integer = 0 To table.SelectPeriod
				Doc.TableColumnHeadings.Add((y + 1).ToString)
			Next

			Dim v As Integer = 0

			For i As Integer = table.IssueAgeMin To table.IssueAgeMax
				ReDim Row(table.SelectPeriod)

				For y As Integer = 0 To table.SelectPeriod
					Row(y) = table.Values(v) * 1000
					v += 1
				Next

				Doc.TableAddRow(i.ToString, "#,##0.00", Row)
			Next

		Else

			If table.StructureCode = TableStructure.AggregateByAge Then
				Doc.TableSubTitleV = "Attained Age"
			Else
				Doc.TableSubTitleV = "Duration"
			End If

			For a As Integer = table.IssueAgeMin To table.MaximumAge
				ReDim Row(table.MaximumAge - table.IssueAgeMin)
				Doc.TableTailAddRow(a.ToString, table.Values(a - table.IssueAgeMin) * 1000)
			Next

		End If

		Return Doc

	End Function

	Public Sub PrintPreview1000q(ByVal printerSettings As PrinterSettings, _
								 ByVal pageSettings As PageSettings, _
								 ByVal table As SOATable)

		Dim PrintDialog As New PrintPreviewDialog()
		PrintDialog.Document = GetDocument1000q(printerSettings, pageSettings, table)
		PrintDialog.Show()

	End Sub

	Public Sub Print1000q(ByVal printerSettings As PrinterSettings, _
						  ByVal pageSettings As PageSettings, _
						  ByVal table As SOATable)

		Dim Doc As PrintTable = GetDocument1000q(printerSettings, pageSettings, table)
		Doc.Print()

	End Sub

	Public Function SelectByUsage(ByVal usages As CheckedListBox.CheckedItemCollection) As SOATableListDecrement

		Dim Tables As New SOATableListDecrement()

		For Each Table As SOATable In Me
			For Each ItemChecked As Object In usages
				If Table.Usage = CType(ItemChecked, ListItem).Index Then
					Tables.Add(Table)
					Exit For
				End If
			Next
		Next

		Return Tables
	End Function

	Public Function SelectByStructure(ByVal structures As CheckedListBox.CheckedItemCollection) As SOATableListDecrement

		Dim Tables As New SOATableListDecrement()

		For Each Table As SOATable In Me
			For Each ItemChecked As Object In structures
				If Table.TableStructureCode = CType(ItemChecked, ListItem).Index Then
					Tables.Add(Table)
					Exit For
				End If
			Next
		Next

		Return Tables
	End Function

	Public Function SelectByCountry(ByVal countries As CheckedListBox.CheckedItemCollection) As SOATableListDecrement

		Dim Tables As New SOATableListDecrement()

		For Each Table As SOATable In Me
			For Each Country As String In countries
				If Table.Country = Country Then
					Tables.Add(Table)
					Exit For
				End If
			Next
		Next

		Return Tables
	End Function

End Class