using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GirlAI))]
public class GirlAIEditor : Editor
{
	private GirlAI m_Controller;

	public override bool RequiresConstantRepaint()
	{
		return true;
	}

	private void OnEnable()
	{
		m_Controller = target as GirlAI;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		Draw();
	}

	private void Draw()
	{
		if (!Application.isPlaying)
		{
			EditorGUILayout.HelpBox("State Machine can only be previewed in Play Mode.", MessageType.Info);
		}
		else
		{
			EditorGUILayout.LabelField("State Machine:", EditorStyles.boldLabel);
			DrawStateMachine(m_Controller.FSM);
		}
	}

	private void DrawStateMachine(FiniteStateMachine stateMachine)
	{
		if (stateMachine != null)
		{
			foreach (AState state in stateMachine.GetStates().Values)
			{
				bool isCurrent = (stateMachine.CurrentStateId == state.Id);
				DrawState(state, isCurrent);
			}
		}
	}

	private void DrawState(AState state, bool isCurrent)
	{
		// draw state name
		string name = (isCurrent ? "▶" : "") + state.ToString();
		EditorGUILayout.LabelField(name);

		// draw substates
		if (state.FSM.HasStates())
		{
			EditorGUI.indentLevel++;
			DrawStateMachine(state.FSM);
			EditorGUI.indentLevel--;
		}
	}
}
